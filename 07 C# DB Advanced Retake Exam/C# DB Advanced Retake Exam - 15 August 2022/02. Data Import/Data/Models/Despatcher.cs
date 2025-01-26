using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[2, 40] (required)
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	Position – text
        [Required]
        public string Position { get; set; } = null!;

        //•	Trucks – collection of type Truck
        public ICollection<Truck> Trucks { get; set; } = new HashSet<Truck>();

    }
}