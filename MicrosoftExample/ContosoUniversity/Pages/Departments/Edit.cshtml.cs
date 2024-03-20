using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Properties.Data;

namespace ContosoUniversity.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Properties.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.DepartmentCreateDto DepartmentDto { get; set; } = default!;


        public SelectList InstructorNameSL { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
  

            var department =  await _context.Departments
                .Include(d=>d.Administrator)
                .FirstOrDefaultAsync(m=>m.DepartmentID==id);
            if (department == null)
            {
                return NotFound();
            }
            DepartmentDto = new Models.DepartmentCreateDto {  Budget= department.Budget, DepartmentID= department.DepartmentID, InstructorID = department.InstructorID, Name = department.Name, StartDate= department.StartDate, ConCurrencyToken = department.ConCurrencyToken};
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FirstMidName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var departmentToUpdate = await _context.Departments.FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (departmentToUpdate == null)
            {
                return HandleDeletedDepartment();
            }

            _context.Entry(departmentToUpdate).Property(
                p=>p.ConCurrencyToken).OriginalValue = DepartmentDto.ConCurrencyToken;

            if(await TryUpdateModelAsync(
                DepartmentDto,
                "DepartmentDto",
                d=> d.Budget,d=> d.Name,d => d.StartDate, d => d.InstructorID
                ))
            {                
                try
                {
                    departmentToUpdate.StartDate = DepartmentDto.StartDate;
                    departmentToUpdate.Budget = DepartmentDto.Budget;
                    departmentToUpdate.InstructorID = DepartmentDto.InstructorID;
                    departmentToUpdate.Name = DepartmentDto.Name;
                    await _context.SaveChangesAsync();
                    return base.RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry= exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "不能保存，这个院系已经在别处删除了");
                        return base.Page();
                    }
                    var dbValues = (Department)databaseEntry.ToObject();
                    await SetDbErrorMessage(dbValues, clientValues, _context);

                    DepartmentDto.ConCurrencyToken = (byte[])dbValues.ConCurrencyToken;
                    ModelState.Remove($"{nameof(DepartmentDto)}.{nameof(DepartmentDto.ConCurrencyToken)}");
                }
            }
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", DepartmentDto.InstructorID);

            return Page();

            
        }

        private IActionResult HandleDeletedDepartment()
        {
            ModelState.AddModelError(string.Empty, "不能保存，这个院系已经在别处删除了");
            InstructorNameSL = new SelectList(_context.Instructors,"ID","FullName",DepartmentDto.InstructorID);
            return Page();
        }

        private async Task SetDbErrorMessage(Department dbValues, Department clientValues, SchoolContext context)
        { 
            if (dbValues.Name!=clientValues.Name) {
                ModelState.AddModelError("Department.Name", $"Current value: {dbValues.Name}");
            }
            if (dbValues.StartDate!=clientValues.StartDate) {
                ModelState.AddModelError("Department.StartDate", $"Current value: {dbValues.StartDate:d}");
            }
            if (dbValues.Budget!=clientValues.Budget) {
                ModelState.AddModelError("Department.Budget", $"Current value: {dbValues.Budget:c}");
            }
            if (dbValues.InstructorID!=clientValues.InstructorID) {
                Instructor dbInstructor = await _context.Instructors.FindAsync(dbValues.InstructorID);
                ModelState.AddModelError("Department.InstructorID", $"Current value: {dbInstructor.FullName}");
            }

            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
              + "was modified by another user after you. The "
              + "edit operation was canceled and the current values in the database "
              + "have been displayed. If you still want to edit this record, click "
              + "the Save button again.");
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}
