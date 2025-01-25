using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        //•	Name – text with length[10…20] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        //•	Rating – double in range[1…10.00] (required)
        [XmlElement("Rating")]
        [Required]
        [Range(1.00,10.00)]
        public double Rating { get; set; }

        //•	YearPublished – integer in range[2018…2023] (required)
        [Required]
        [Range(2018,2023)]
        public int YearPublished { get; set; }

        //•	CategoryType – enumeration of type CategoryType, with possible values
        //(Abstract, Children, Family, Party, Strategy) (required)
        [Required]
        [Range(0,4)]
        public int CategoryType { get; set; }


        //•	Mechanics – text(string, not an array) (required)
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}