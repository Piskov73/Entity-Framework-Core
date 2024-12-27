using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        //•	ManufacturerName – unique text with length[4…40] (required)
        [XmlElement("ManufacturerName")]
        [Required]
        [MinLength(4)]
        [MaxLength(40)]
        public string ManufacturerName { get; set; } = null!;

        //•	Founded – text with length[10…100] (required)
        [XmlElement("Founded")]
        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Founded { get; set; } = null!;
    }
}
