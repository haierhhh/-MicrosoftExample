using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Models;
using ContosoUniversity.Properties.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.Properties.Data.SchoolContext _context;

        public SelectList InstructorNameSL { get; set; }
        [BindProperty]
        public Models.DepartmentCreateDto DepartmentDto { get; set; } = default!;
        public CreateModel(ContosoUniversity.Properties.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName");
            return Page();
        }

        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var entry = _context.Departments.Add(new Department());
            entry.CurrentValues.SetValues(DepartmentDto);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
    public class DepartmentCreateDto
    {
        public int DepartmentID { get; set; }
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }

    }
}

