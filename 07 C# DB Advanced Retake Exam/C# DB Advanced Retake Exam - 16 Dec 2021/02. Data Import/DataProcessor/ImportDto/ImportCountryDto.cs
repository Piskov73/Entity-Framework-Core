using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDto
    {
        //•	CountryName – text with length[4, 60] (required)
        [XmlElement("CountryName")]
        [Required]
        [MinLength(4)]
        [MaxLength(60)]
        public string CountryName { get; set; } = null!;

        //•	ArmySize – integer in the range[50_000….10_000_000] (required)
        [XmlElement("ArmySize")]
        [Required]
        [Range(50_000,10_000_000)]
        public int ArmySize { get; set; }
    }
}
