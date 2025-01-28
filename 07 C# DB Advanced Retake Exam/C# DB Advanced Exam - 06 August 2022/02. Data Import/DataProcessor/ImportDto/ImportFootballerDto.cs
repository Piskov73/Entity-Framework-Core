using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        //•	Name – text with length[2, 40] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	ContractStartDate – date and time(required)
        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; } = null!;

        //•	ContractEndDate – date and time(required)
        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; } = null!;

        //•	PositionType - enumeration of type PositionType, with possible
        //values(Goalkeeper, Defender, Midfielder, Forward) (required)
        [XmlElement("PositionType")]
        [Required]
        [Range(0,3)]
        public int PositionType { get; set; }

        //•	BestSkillType – enumeration of type BestSkillType, with possible
        //values(Defence, Dribble, Pass, Shoot, Speed) (required)
        [XmlElement("BestSkillType")]
        [Required]
        [Range(0,4)]
        public int BestSkillType { get; set; }
    }
}