using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Cell
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	CellNumber – integer in the range[1, 1000] (required)
        [Required]
        public int CellNumber { get; set; }

        //•	HasWindow – bool (required)
        [Required]
        public bool HasWindow { get; set; }

        //•	DepartmentId – integer, foreign key(required)
        [Required]
        public int DepartmentId { get; set; }

        //•	Department – the cell's department (required)
        [Required]
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } = null!;

        //•	Prisoners – collection of type Prisoner
        public ICollection<Prisoner> Prisoners { get; set; } = new HashSet<Prisoner>();

    }
}