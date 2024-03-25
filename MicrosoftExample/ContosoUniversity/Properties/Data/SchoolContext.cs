using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Components.Web;

namespace ContosoUniversity.Properties.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        public DbSet<CourseAssignment> CourseAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable(nameof(Course))
                .HasMany(c=>c.Instructors)
                .WithMany(i=>i.Courses);
            modelBuilder.Entity<Enrollment>().ToTable(nameof(Enrollment));
            modelBuilder.Entity<Student>().ToTable(nameof(Student));
            modelBuilder.Entity<Instructor>().ToTable(nameof(Instructor));
            modelBuilder.Entity<Department>().ToTable(nameof(Department));
            modelBuilder.Entity<OfficeAssignment>().ToTable(nameof(OfficeAssignment));
            modelBuilder.Entity<CourseAssignment>().ToTable(nameof(CourseAssignment));
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c=>new { c.CourseID,c.InstructorID});
            //modelBuilder.Entity<Department>()
            //    .HasOne(d=>d.Administrator)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
