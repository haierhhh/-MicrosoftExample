using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.Extensions.Configuration;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;
        private readonly IConfiguration Configration;

        public IndexModel(ContosoUniversity.Data.SchoolContext context,IConfiguration configuration)
        {
            _context = context;
            Configration = configuration;
        }

        public PaginatedList<Student> StudentList { get;set; } = default!;

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString,int?pageIndex )
        {
            CurrentSort = sortOrder;
            NameSort = string.IsNullOrEmpty(sortOrder)?"name_desc":"" ;
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString!=null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter; //这里是为了在操作之后，把当前过滤的关键字 赋值给searchString,可以起到记录的作用，页面文本框searchString的值 在跳转当前页面后还保留
            }
            currentFilter = searchString;
            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;
            if(!string.IsNullOrEmpty(searchString))
            {
                
                studentsIQ=studentsIQ.Where(s=>s.LastName.Contains(searchString)
                                            ||s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }
            var pageSize = Configration.GetValue("PageSize", 4);
            StudentList = await PaginatedList<Student>.CreateAsync(
                    studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
