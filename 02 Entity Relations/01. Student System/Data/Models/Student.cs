using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [Column(TypeName ="NVARCHAR(100)")]
        public string Name { get; set; } = null!;
        [MinLength(10)]
        [MaxLength(10)]
        [Column(TypeName = "VARCHAR")]
        public string? PhoneNumber { get; set; }
        [Required]
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; } = null!;
        public virtual ICollection<Homework> Homeworks { get; set; }= null!;

    }
}
