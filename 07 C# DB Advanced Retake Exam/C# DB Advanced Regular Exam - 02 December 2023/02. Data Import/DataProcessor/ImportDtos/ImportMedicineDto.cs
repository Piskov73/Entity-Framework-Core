using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicineDto
    {
        //•	Name – text with length[3, 150] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        //•	Price – decimal in range[0.01…1000.00] (required)
        [XmlElement("Price")]
        [Required]
        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }

        //•	Category – Category enum (Analgesic = 0, Antibiotic, Antiseptic, Sedative, Vaccine) (required)
        [XmlAttribute("category")]
        [Required]
        [Range(0, 4)]
        public int Category { get; set; }

        //•	ProductionDate – DateTime(required)
        [XmlElement("ProductionDate")]
        [Required]
        public string ProductionDate { get; set; } = null!;

        //•	ExpiryDate – DateTime(required)
        [XmlElement("ExpiryDate")]
        [Required]
        public string ExpiryDate { get; set; } = null!;

        //•	Producer – text with length[3, 100] (required)
        [XmlElement("Producer")]
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Producer { get; set; } = null!;

    }
}