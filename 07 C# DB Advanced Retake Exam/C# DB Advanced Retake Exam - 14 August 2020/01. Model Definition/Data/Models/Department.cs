using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace SoftJail.Data.Models
{
    public class Department
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with min length 3 and max length 25 (required)
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        //•	Cells - collection of type Cell
        public ICollection<Cell> Cells { get; set; } = new HashSet<Cell>();
        
    }
}