using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class ImportPropertyDto
    {

        //•	PropertyIdentifier – text with length[16, 20] (required)
        [XmlElement("PropertyIdentifier")]
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; } = null!;

        //•	Area – int not negative(required)
        [XmlElement("Area")]
        [Required]
        [Range(1,int.MaxValue)]
        public int Area { get; set; }

        //•	Details - text with length[5, 500] (not required)
        [XmlElement("Details")]
        [MinLength(5)]
        [MaxLength(500)]
        public string? Details { get; set; }

        //•	Address – text with length[5, 200] (required)
        [XmlElement("Address")]
        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        //•	DateOfAcquisition – DateTime(required)
        [XmlElement("DateOfAcquisition")]
        [Required]
        [MaxLength(10)]
        public string DateOfAcquisition { get; set; } = null!;
    }
}