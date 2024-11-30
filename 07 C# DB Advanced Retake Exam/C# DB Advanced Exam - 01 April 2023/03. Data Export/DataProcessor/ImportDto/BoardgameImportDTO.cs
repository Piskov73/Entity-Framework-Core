namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using static Shared.Constants;

    [XmlType("Boardgame")]
    public class BoardgameImportDTO
    {
        //•	Name – text with length[10…20] (required)
        [Required]
        [XmlElement("Name")]
        [MinLength(NameBoardgameMinLenght)]
        [MaxLength(NameBoardgameMaxLenght)]
        public string Name { get; set; } = null!;

        //•	Rating – double in range[1…10.00] (required)
        [Required]
        [XmlElement("Rating")]
        [Range(RatingBoardgameMin,RatingBoardgameMax)]
        public double Rating { get; set; }

        //•	YearPublished – integer in range[2018…2023] (required)
        [Required]
        [XmlElement("YearPublished")]
        [Range(YearPublishedBoardgameMin,YearPublishedBoardgameMax)]
        public int YearPublished { get; set; }

        //•	CategoryType – enumeration of type CategoryType
        //, with possible values(Abstract, Children, Family, Party, Strategy) (required)
        [Required]
        [XmlElement("CategoryType")]
        [Range(0,4)]
        public int CategoryType { get; set; }

        //•	Mechanics – text(string, not an array) (required)
        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; } = null!;
    }
}
