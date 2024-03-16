using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Properties.Data;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexSelectModel : PageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;

        public IndexSelectModel(ContosoUniversity.Properties.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<CourseViewModel> CourseVM { get;set; } = default!;

        public async Task OnGetAsync()
        {
            CourseVM = await _context.Courses
                .Select(s=> new CourseViewModel { CourseID=s.CourseID, Credits=s.Credits, Title=s.Title, DepartmentName=s.Department.Name})
                .ToListAsync();
 
        }
    }
}
