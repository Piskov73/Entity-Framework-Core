namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Constants.Constants;

    [XmlType("Client")]
    public class ClientImportDTO
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(ClientNameMinLength)]
        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("NumberVat")]
        [MinLength(NumberVatMinLength)]
        [MaxLength(NumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        [XmlArray("Addresses")]
        public AddressImportDTO[] Addresses = null!;
    }
}
