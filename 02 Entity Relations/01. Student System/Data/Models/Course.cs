using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(80)")]
        public string Name { get; set; } = null!;

        [Column(TypeName ="NVARCHAR(1000)")]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName ="DECIMAL(18,4)")]
        public decimal Price { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; } = null!;
        public virtual ICollection<Resource> Resources { get; set; } = null!;
        public virtual ICollection<Homework> Homeworks { get; set; } = null!;

    }
}
