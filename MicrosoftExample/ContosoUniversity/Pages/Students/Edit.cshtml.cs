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
using System.Diagnostics;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly SchoolContext _context;

        public EditModel(SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentVM StudentVM { get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Select(s => new StudentVM { ID = s.ID, LastName = s.LastName, FirstMidName = s.FirstMidName, EnrollmentDate = s.EnrollmentDate })
                .FirstOrDefaultAsync(m => m.ID == id);
            //var student =  await _context.Students.FindAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            StudentVM=student;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Attach(StudentList).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StudentExists(StudentList.ID))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToPage("./Index");
        //}
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var studentToUpdate = await _context.Students.FindAsync(id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            if(await TryUpdateModelAsync<StudentVM>(
                StudentVM,
                "StudentVM",
                s=>s.LastName,s=>s.FirstMidName,s=>s.EnrollmentDate
                ))
            {
                studentToUpdate.LastName = StudentVM.LastName;
                studentToUpdate.FirstMidName = StudentVM.FirstMidName;
                studentToUpdate.EnrollmentDate = StudentVM.EnrollmentDate;
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
                //foreach (var modelstate in ViewData.ModelState.Values)
                //{
                //    foreach(var error in modelstate.Errors) {
                //        Debug.WriteLine(error.ErrorMessage);
                //    }
                //}
            }

            return Page();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
