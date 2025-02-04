using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public  class ImportPlayDto
    {
        //•	Title – text with length[4, 50] (required)
        [XmlElement("Title")]
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        //•	Duration – TimeSpan in format {hours:minutes:seconds}, with a minimum length of 1 hour. (required)
        [XmlElement("Duration")]
        [Required]
        public string Duration { get; set; } = null!;

        //•	Rating – float in the range[0.00….10.00] (required)
        [XmlElement("Raiting")]
        [Required]
        [Range(0.00, 10.00)]
        public float Rating { get; set; }

        //•	Genre – enumeration of type Genre, with possible values(Drama, Comedy, Romance, Musical) (required)
        [XmlElement("Genre")]
        [Required]
        public string Genre { get; set; } = null!;

        //•	Description – text with length up to 700 characters(required)
        [XmlElement("Description")]
        [Required]
        [MaxLength(700)]
        public string Description { get; set; } = null!;

        //•	Screenwriter – text with length[4, 30] (required)
        [XmlElement("Screenwriter")]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Screenwriter { get; set; } = null!;
    }
}
