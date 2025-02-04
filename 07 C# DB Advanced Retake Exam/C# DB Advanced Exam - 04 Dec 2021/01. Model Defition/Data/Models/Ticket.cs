using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theatre.Data.Models
{
    public class Ticket
    {

        //  •	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Price – decimal in the range[1.00….100.00] (required)
        [Required]
        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        //•	RowNumber – sbyte in range[1….10] (required)
        [Required]
        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        //•	PlayId – integer, foreign key(required)
        [Required]
        public int PlayId { get; set; }
        [ForeignKey(nameof(PlayId))]
        public Play Play { get; set; } = null!;

        //•	TheatreId – integer, foreign key(required)
        [Required]
        public int TheatreId { get; set; }
        [ForeignKey(nameof(TheatreId))]
        public Theatre Theatre { get; set; } = null!;

    }
}