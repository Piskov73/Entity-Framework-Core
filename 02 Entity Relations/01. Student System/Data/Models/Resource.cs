using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace P01_StudentSystem.Data.Models
{
    [Table("Resource")]
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Url { get; set; } = null!;

       
        [Required]
        public ResourceType ResourceType { get; set; } 
        
        
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }=null!;

    }
    public enum ResourceType
    {
        Video,
        Presentation,
        Document,
        Other
    }

}
