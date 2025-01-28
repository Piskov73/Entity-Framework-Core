using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        //•	Name – text with length[2, 40] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	Nationality – text(required)
        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; } = null!;

        //•	Footballers – collection of type Footballer
        [XmlArray("Footballers")]
        [XmlArrayItem("Footballer")]
        public List<ImportFootballerDto> Footballers { get; set; } = new List<ImportFootballerDto>();
    }
}
