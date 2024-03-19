using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Models
{
    [Bind("LastName,FirstMidName,EnrollmentDate")]
    public class StudentVM
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
