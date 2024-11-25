namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Constants.Constants;

    [XmlType("Address")]
    public class AddressImportDTO
    {
        [Required]
        [XmlElement("StreetName")]
        [MinLength(MinLengthStreetName)]
        [MaxLength(MaxLengthStreetName)]
        public string StreetName { get; set; } = null!;

        [Required]
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }

        [Required]
        [XmlElement("PostCode")]
        public string PostCode { get; set; } = null!;

        [Required]
        [XmlElement("City")]
        [MinLength(MinLenghtCityName)]
        [MaxLength(MaxLenghtCityName)]
        public string City { get; set; } = null!;

        [Required]
        [XmlElement("Country")]
        [MinLength(MinLenghtCountryName)]
        [MaxLength(MaxLenghtCountryName)]
        public string Country { get; set; } = null!;



    }
}
