using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {

        //•	Name – text with length[2, 80] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;

        //•	PostalCode – text with length 8.
        //All postal codes must have the following structure:starting with two capital letters,
        //followed by e dash '-', followed by five digits.Example: SF-10000 (required)
        [XmlElement("PostalCode")]
        [Required]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
        public string PostalCode { get; set; } = null!;

        //•	Region – Region enum (SouthEast = 0, SouthWest, NorthEast, NorthWest) (required)
        [XmlAttribute("Region")]
        [Required]
        public string Region { get; set; } = null!;

        //•	Properties - collection of type Property
        [XmlArray("Properties")]
        [XmlArrayItem("Property")]
        public ImportPropertyDto[] Properties { get; set; } = new ImportPropertyDto[] { };
    }
}
