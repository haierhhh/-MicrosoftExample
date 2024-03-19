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
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

namespace ContosoUniversity.Pages.Instructors
{
    public class EditModel : InstructorCoursesPageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Properties.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InstructorVM InstructorVM { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor =  await _context.Instructors
                .Include(i=>i.OfficeAssignment)
                .Include(i=>i.Courses)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            InstructorVM = new InstructorVM {  FirstMidName=instructor.FirstMidName,LastName=instructor.LastName,HireDate=instructor.HireDate,OfficeAssignmentLocation=instructor.OfficeAssignment?.Location};

            PopulateAssignedCourseData(_context, instructor);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (instructorToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<InstructorVM>(
                InstructorVM,
                "InstructorVM",
                i => i.FirstMidName, i => i.LastName,
                i => i.HireDate))
            {
                if (String.IsNullOrWhiteSpace(
                    InstructorVM.OfficeAssignmentLocation))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }
                else
                {
                    instructorToUpdate.OfficeAssignment= new OfficeAssignment { Location = InstructorVM.OfficeAssignmentLocation, InstructorID=instructorToUpdate.ID, Instructor=instructorToUpdate };
                }
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                instructorToUpdate.FirstMidName = InstructorVM.FirstMidName;
                instructorToUpdate.LastName = InstructorVM.LastName;
                instructorToUpdate.HireDate = InstructorVM.HireDate;
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(_context, instructorToUpdate);
            return Page();
        }

        public void UpdateInstructorCourses(string[] selectedCourses,
                                            Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach (var course in _context.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        var courseToRemove = instructorToUpdate.Courses.Single(
                                                        c => c.CourseID == course.CourseID);
                        instructorToUpdate.Courses.Remove(courseToRemove);
                    }
                }
            }
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
