using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Models;
using ContosoUniversity.Properties.Data;
using System.Diagnostics;

namespace ContosoUniversity.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Properties.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateDepartmentsDropDownList(_context);
        //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
            return Page();
        }

        [BindProperty]
        public CourseVM CourseVM { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            var emptyCourse = new Course();
 

            if (await TryUpdateModelAsync<CourseVM>(
                CourseVM,"course",
                s=>s.CourseID,s=>s.DepartmentID,s => s.Title,s => s.Credits))
            {

                //foreach (var modelState in ViewData.ModelState.Values)
                //{
                //    foreach (var error in modelState.Errors)
                //    {
                //        Debug.WriteLine(error.ErrorMessage);
                //    }
                //}

                var entry= _context.Courses.Add(emptyCourse);
                entry.CurrentValues.SetValues(CourseVM);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateDepartmentsDropDownList(_context, emptyCourse.DepartmentID);

            return Page();

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Courses.Add(CourseVM);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
        }
    }
}
