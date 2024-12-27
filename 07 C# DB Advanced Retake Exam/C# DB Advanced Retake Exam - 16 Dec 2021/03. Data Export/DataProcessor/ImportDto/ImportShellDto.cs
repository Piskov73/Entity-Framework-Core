using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImportShellDto
    {
        //•	ShellWeight – double in range[2…1_680] (required)
        [XmlElement("ShellWeight")]
        [Required]
        [Range(2.00,1680.00)]
        public double ShellWeight { get; set; }

        //•	Caliber – text with length[4…30] (required)
        [XmlElement("Caliber")]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Caliber { get; set; } = null!;
    }
}
