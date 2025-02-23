using System.ComponentModel.DataAnnotations;

using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDto
{
    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlAttribute("title")]
        [Required]
        public string Game { get; set; } = null!;

        //•	Type – enumeration of type PurchaseType, with possible values("Retail", "Digital") (required)
        [XmlElement("Type")]
        [Required]
        public string Type { get; set; } = null!;

        //•	ProductKey – text, which consists of 3 pairs of 4 uppercase Latin letters and digits,
        //separated by dashes(ex. "ABCD-EFGH-1J3L") (required)
        [XmlElement("Key")]
        [Required]
        [MaxLength(14)]
        [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string ProductKey { get; set; } = null!;

        //•	Date – Date(required)
        [XmlElement("Date")]
        [Required]
        public string Date { get; set; } = null!;
        [XmlElement("Card")]
        [Required]
        public string Card { get; set; } = null!;

       

    }
}
