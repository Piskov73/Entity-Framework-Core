using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastDto
    {
        //•	FullName – text with length[4, 30] (required)
        [XmlElement("FullName")]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string FullName { get; set; } = null!;

        //•	IsMainCharacter – Boolean represents if the actor plays the main character in a play(required)
        [XmlElement("IsMainCharacter")]
        [Required]
        [RegularExpression(@"^(true|false)$")]
        public string IsMainCharacter { get; set; } = null!;

        //•	PhoneNumber – text in the following format: "+44-{2 numbers}-{3 numbers}-{4 numbers}".
        //Valid phone numbers are: +44-53-468-3479, +44-91-842-6054, +44-59-742-3119 (required)
        [XmlElement("PhoneNumber")]
        [Required]
        [RegularExpression(@"^\+44-\d{2}-\d{3}-\d{4}$")]
        public string PhoneNumber { get; set; } = null!;

        //•	PlayId – integer, foreign key(required)
        [XmlElement("PlayId")]
        [Required]
        public int PlayId { get; set; }
    }
}
