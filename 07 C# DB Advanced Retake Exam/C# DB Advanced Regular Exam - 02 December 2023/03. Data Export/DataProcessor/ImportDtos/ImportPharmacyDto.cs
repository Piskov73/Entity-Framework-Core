
using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {

        //•	Name – text with length[2, 50] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        //•	PhoneNumber – text with length 14. (required)
        //o   All phone numbers must have the following structure: three digits enclosed in parentheses,
        //followed by a space, three more digits,
        //a hyphen, and four final digits: 
        //	Example -> (123) 456-7890
        [XmlElement("PhoneNumber")]
        [Required]
        [MaxLength(14)]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$")]
        public string PhoneNumber { get; set; } = null!;

        //•	IsNonStop – bool (required)
        [XmlAttribute("non-stop")]
        [Required]
        public string IsNonStop { get; set; } = null!;

        //•	Medicines - collection of type Medicine
        [XmlArray("Medicines")]
        [XmlArrayItem("Medicine")]
        public List<ImportMedicineDto> Medicines { get; set; } = new List<ImportMedicineDto>();
    }
}
