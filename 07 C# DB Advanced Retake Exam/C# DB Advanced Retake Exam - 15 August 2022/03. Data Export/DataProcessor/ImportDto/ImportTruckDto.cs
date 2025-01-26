using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckDto
    {
        //•	RegistrationNumber – text with length 8
        //. First two characters are upper letters[A - Z],
        //followed by four digits and the last two characters are upper letters[A - Z] again.
        [XmlElement("RegistrationNumber")]
        [Required]
        [RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$")]
        [MaxLength(8)]
        public string RegistrationNumber { get; set; } = null!;

        //•	VinNumber – text with length 17 (required)
        [XmlElement("VinNumber")]
        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        public string VinNumber { get; set; } = null!;

        //•	TankCapacity – integer in range[950…1420]
        [XmlElement("TankCapacity")]
        [Required]
        [Range(950,1420)]
        public int TankCapacity { get; set; }

        //•	CargoCapacity – integer in range[5000…29000]
        [XmlElement("CargoCapacity")]
        [Required]
        [Range(5000,29000)]
        public int CargoCapacity { get; set; }

        //•	CategoryType – enumeration of type CategoryType, with possible values
        //(Flatbed, Jumbo, Refrigerated, Semi) (required)
        [XmlElement("CategoryType")]
        [Required]
        [Range(0,3)]
        public int CategoryType { get; set; }

        //•	MakeType – enumeration of type MakeType, with possible values
        //(Daf, Man, Mercedes, Scania, Volvo) (required)
        [XmlElement("MakeType")]
        [Required]
        [Range(0,4)]
        public int MakeType { get; set; }
    }
}