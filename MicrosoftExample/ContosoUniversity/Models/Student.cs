using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name="Last Name")]        
        public string LastName { get; set; }
        [Required]
        [StringLength(50,ErrorMessage ="第一个名字的长度不能大于50")]
        [Column("FirstName")]
        [Display(Name ="First Name")]
        public string FirstMidName { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
        //public string Secret { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
