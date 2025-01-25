using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        //•	FirstName – text with length[2, 7] (required) 
        [XmlElement("FirstName")]
        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string FirstName { get; set; } = null!;

        //•	LastName – text with length[2, 7] (required)
        [XmlElement("LastName")]
        [Required]
        [MinLength(2)]
        [MaxLength(7)]
        public string LastName { get; set; } = null!;

        //•	Boardgames – collection of type Boardgame
        [XmlArray("Boardgames")]
        [XmlArrayItem("Boardgame")]
        public List<ImportBoardgameDto> Boardgames { get; set; } = new List<ImportBoardgameDto>();
    }
}
