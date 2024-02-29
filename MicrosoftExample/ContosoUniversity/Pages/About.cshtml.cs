using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using ContosoUniversity.Properties.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages
{
    public class AboutModel : PageModel
    {
        private readonly SchoolContext _context;

        public AboutModel(SchoolContext context)
        {
            _context = context;
        }

        public IList<EnrollmentDateGroup> EnrollmentDateGroups { get; set; }
        public async Task OnGet()
        {
            IQueryable<EnrollmentDateGroup> data =
                from student in _context.Students
                group student by new {student.EnrollmentDate} into dateGroup
                select new EnrollmentDateGroup
                {
                    EnrollmentDate = dateGroup.Key.EnrollmentDate,
                    EnrollmentCount = dateGroup.Count()
                };

              EnrollmentDateGroups= await data.AsNoTracking().ToListAsync();
        }
    }
}
