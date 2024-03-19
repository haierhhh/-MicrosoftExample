using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Models;
using ContosoUniversity.Properties.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors
{
    public class CreateModel : InstructorCoursesPageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;
        private readonly ILogger<InstructorCoursesPageModel> _logger;

        public CreateModel(ContosoUniversity.Properties.Data.SchoolContext context,ILogger<InstructorCoursesPageModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var instructor = new Instructor();
            instructor.Courses= new List<Course>();
            PopulateAssignedCourseData(_context,instructor);
            return Page();
        }

        [BindProperty]
        public InstructorVM InstructorVM { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
           var newInstructor = new Instructor();
            if (selectedCourses.Length > 0)
            {
                newInstructor.Courses = new List<Course>();

                _context.Courses.Load();
            }

            foreach (var course in selectedCourses)
            {
                var foundCourse = await _context.Courses.FindAsync(int.Parse(course));
                if (foundCourse != null)
                {
                    newInstructor.Courses.Add(foundCourse);
                }
                else
                {
                    _logger.LogWarning("课程{course}没找到", course);
                }
            }

            try
            {
                if (await TryUpdateModelAsync<InstructorVM>(
                    InstructorVM,
                    "InstructorVM",
                    i => i.FirstMidName, i => i.LastName,
                                i => i.HireDate, i => i.OfficeAssignmentLocation))
                {
                    newInstructor.FirstMidName=InstructorVM.FirstMidName;
                    newInstructor.LastName=InstructorVM.LastName;
                    newInstructor.HireDate=InstructorVM.HireDate;
                    newInstructor.OfficeAssignment = new OfficeAssignment { Instructor=newInstructor, Location=InstructorVM.OfficeAssignmentLocation };
                    _context.Instructors.Add(newInstructor);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

            PopulateAssignedCourseData(_context,newInstructor);
            return Page();
        }
    }
}
