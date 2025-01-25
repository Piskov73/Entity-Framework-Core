using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientDto
    {
        //•	Name – text with length[10…25] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        //•	NumberVat – text with length[10…15] (required)
        [XmlElement("NumberVat")]
        [Required]
        [MinLength(10)]
        [MaxLength(15)]
        public string NumberVat { get; set; } = null!;

        //•	Addresses – collection of type Address
        [XmlArray("Addresses")]
        [XmlArrayItem("Address")]
        public HashSet<ImportAddressDto> Addresses { get; set; } = new HashSet<ImportAddressDto>();
    }
}
