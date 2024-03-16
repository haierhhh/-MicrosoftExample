using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Properties.Data;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Properties.Data.SchoolContext context)
        {
            _context = context;
        }

        public InstructorIndexData InstructorIndexData { get; set; }

        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id,int? courseID)
        {
            InstructorIndexData = new InstructorIndexData();
            InstructorIndexData.Instructors= await _context.Instructors
                .Include(i=>i.OfficeAssignment)
                .Include(i=>i.Courses)
                    .ThenInclude(d=>d.Department)
                .OrderBy(o=>o.LastName)
                .ToListAsync();
            if (id != null)
            {
                InstructorID=id.Value;
                Instructor instructor=InstructorIndexData.Instructors.Where(w=>w.ID==id.Value).Single();
                InstructorIndexData.Courses=instructor.Courses;
            }
            if(courseID != null)
            {
                CourseID=courseID.Value;
                IEnumerable<Enrollment> enrollments=await _context.Enrollments
                    .Where(w=>w.CourseID==courseID.Value)
                    .Include(s=>s.Student)
                    .ToListAsync();

                InstructorIndexData.Enrollments=enrollments;
            }
            
        }
    }
}
