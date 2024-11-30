namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using static Shared.Constants;
    [XmlType("Creator")]
    public class CreatorImportDTO
    {
        //•	FirstName – text with length[2, 7] (required) 
        [Required]
        [XmlElement("FirstName")]
        [MinLength(CreatorFirstNameMinLength)]
        [MaxLength(CreatorFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        //•	LastName – text with length[2, 7] (required)
        [Required]
        [XmlElement("LastName")]
        [MinLength(CreatorLasrNameMinLength)]
        [MaxLength(CreatorLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        //•	Boardgames – collection of type Boardgame

        [XmlArray("Boardgames")]
        public HashSet<BoardgameImportDTO> Boardgames { get; set; } = new HashSet<BoardgameImportDTO>();
    }

}
