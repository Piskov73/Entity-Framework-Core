using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class ImportAddressDto
    {
        //•	StreetName – text with length[10…20] (required)
        [XmlElement("StreetName")]
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string StreetName { get; set; } = null!;

        //•	StreetNumber – integer(required)
        [XmlElement("StreetNumber")]
        [Required]
        public int StreetNumber { get; set; }

        //•	PostCode – text(required)
        [XmlElement("PostCode")]
        [Required]
        public string PostCode { get; set; } = null!;

        //•	City – text with length[5…15] (required)
        [XmlElement("City")]
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string City { get; set; } = null!;

        //•	Country – text with length[5…15] (required)
        [XmlElement("Country")]
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Country { get; set; } = null!;
    }
}