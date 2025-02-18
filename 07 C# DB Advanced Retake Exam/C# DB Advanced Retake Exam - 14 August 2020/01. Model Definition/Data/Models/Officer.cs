using SoftJail.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	FullName – text with min length 3 and max length 30 (required)
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;

        //•	Salary – decimal (non-negative, minimum value: 0) (required)
        [Required]
        public decimal Salary { get; set; }

        //•	Position – Position enumeration with possible values: "Overseer, Guard, Watcher, Labour" (required)
        [Required]
        public Position Position { get; set; }

        //•	Weapon – Weapon enumeration with possible values: "Knife, FlashPulse, ChainRifle, Pistol, Sniper" (required)
        [Required]
        public Weapon Weapon { get; set; }

        //•	DepartmentId – integer, foreign key(required)
        [Required]
        public int DepartmentId { get; set; }

        //•	Department – the officer's department (required)
        [Required]
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        //•	OfficerPrisoners – collection of type OfficerPrisoner
        public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; } = new HashSet<OfficerPrisoner>();
    }
}
