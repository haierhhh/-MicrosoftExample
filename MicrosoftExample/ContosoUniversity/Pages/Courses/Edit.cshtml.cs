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

namespace ContosoUniversity.Pages.Courses
{
    public class EditModel : DepartmentNamePageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Properties.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CourseViewModel CourseVM { get; set; } = default!;

        private Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course =  await _context.Courses
                .Include(c=>c.Department)                
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            CourseVM = new CourseViewModel { CourseID=course.Credits, Credits=course.Credits,DepartmentID=course.DepartmentID,DepartmentName=course.Department.Name, Title= course.Title};
            PopulateDepartmentsDropDownList(_context, course.DepartmentID);
           //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
            return Page();
        }

        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Attach(CourseVM).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CourseExists(CourseVM.CourseID))
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
            if (id==null)
            {
                return NotFound();
            }
            var courseToUpdate = await _context.Courses.FindAsync(id);

            if(courseToUpdate == null)
            {
                return NotFound();
            }

            //if (!await TryUpdateModelAsync<CourseViewModel>(CourseVM, "CourseVM", c => c.Title, c => c.Credits, c => c.DepartmentID))
            //{
            //    foreach (var modelstate in ViewData.ModelState.Values)
            //    {
            //        foreach (var error in modelstate.Errors)
            //        {
            //            Debug.WriteLine(error.ErrorMessage);
            //        }
            //    }
            //}
            if (await TryUpdateModelAsync<CourseViewModel>(CourseVM, "CourseVM", c => c.Title, c => c.Credits, c => c.DepartmentID))
            {
                courseToUpdate.Title =CourseVM.Title;
                courseToUpdate.Credits =CourseVM.Credits;
                courseToUpdate.DepartmentID =CourseVM.DepartmentID;
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateDepartmentsDropDownList(_context, courseToUpdate.DepartmentID);
            return Page();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
