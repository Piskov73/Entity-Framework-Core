using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace Theatre.Data.Models
{
    public class Play
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Title – text with length[4, 50] (required)
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        //•	Duration – TimeSpan in format {hours:minutes:seconds}, with a minimum length of 1 hour. (required)
        [Required]
        public TimeSpan Duration { get; set; }

        //•	Rating – float in the range[0.00….10.00] (required)
        [Required]
        [Range(0.00, 10.00)]
        public float Rating { get; set; }

        //•	Genre – enumeration of type Genre, with possible values(Drama, Comedy, Romance, Musical) (required)
        [Required]
        public Genre Genre { get; set; }

        //•	Description – text with length up to 700 characters(required)
        [Required]
        [MaxLength(700)]
        public string Description { get; set; } = null!;

        //•	Screenwriter – text with length[4, 30] (required)
        [Required]
        [MaxLength(30)]
        public string Screenwriter { get; set; } = null!;

        //•	Casts – a collection of type Cast
        public ICollection<Cast> Casts { get; set; } = new HashSet<Cast>();

        //•	Tickets – a collection of type Ticket
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
