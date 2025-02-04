using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto
{
    public  class ImportTheatreDto
    {
        //•	Name – text with length[4, 30] (required)
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        //•	NumberOfHalls – sbyte between[1…10] (required)
        [Required]
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        //•	Director – text with length[4, 30] (required)
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Director { get; set; } = null!;

        //•	Tickets – a collection of type Ticket
        public ImportTicketDto[] Tickets { get; set; } = new ImportTicketDto[] { };
    }
}
