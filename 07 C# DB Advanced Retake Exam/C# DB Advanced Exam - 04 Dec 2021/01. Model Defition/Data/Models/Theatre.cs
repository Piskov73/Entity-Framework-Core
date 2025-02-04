using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Theatre.Data.Models
{
    public class Theatre
    {

        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[4, 30] (required)
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        //•	NumberOfHalls – sbyte between[1…10] (required)
        [Required]
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        //•	Director – text with length[4, 30] (required)
        [Required]
        [MaxLength(30)]
        public string Director { get; set; } = null!;

        //•	Tickets – a collection of type Ticket
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
