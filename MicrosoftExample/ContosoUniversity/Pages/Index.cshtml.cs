using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SchoolContext _dbContext;

        public string Message { get; set; }

        public IndexModel(ILogger<IndexModel> logger,SchoolContext schoolContext)
        {
            _logger = logger;
            _dbContext = schoolContext;
        }

        public async void OnGetAsync()
        {
            
            Message = await TestConvertToString<Student>.ConvertString(_dbContext.Students.AsNoTracking());
        }
    }
}
