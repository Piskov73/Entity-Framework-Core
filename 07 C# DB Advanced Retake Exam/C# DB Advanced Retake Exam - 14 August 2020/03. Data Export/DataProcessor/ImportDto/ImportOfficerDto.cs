using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerDto
    {
        //•	FullName – text with min length 3 and max length 30 (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;

        //•	Salary – decimal (non-negative, minimum value: 0) (required)
        [XmlElement("Money")]
        [Required]
        [Range(0,double.MaxValue)]
        public decimal Salary { get; set; }

        //•	Position – Position enumeration with possible values: "Overseer, Guard, Watcher, Labour" (required)
        [XmlElement("Position")]
        [Required]
        public string Position { get; set; } = null!;

        //•	Weapon – Weapon enumeration with possible values:
        //"Knife, FlashPulse, ChainRifle, Pistol, Sniper" (required)
        [XmlElement("Weapon")]
        [Required]
        public string Weapon { get; set; } = null!;

        //•	DepartmentId – integer, foreign key(required)
        [XmlElement("DepartmentId")]
        [Required]
        public int DepartmentId { get; set; }



        //•	OfficerPrisoners – collection of type OfficerPrisoner
        [XmlArray("Prisoners")]
        [XmlArrayItem("Prisoner")]

        public ImportPrisonerIdDto[] Prisoners { get; set; } = new ImportPrisonerIdDto[] { };
    }
}
