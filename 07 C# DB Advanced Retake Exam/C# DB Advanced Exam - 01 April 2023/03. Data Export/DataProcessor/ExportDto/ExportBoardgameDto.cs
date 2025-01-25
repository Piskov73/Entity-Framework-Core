using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportBoardgameDto
    {
        //<Boardgame>

        //<BoardgameName>Great Western Trail</BoardgameName>
        [XmlElement("BoardgameName")]
        public string BoardgameName { get; set; } = null!;

        //<BoardgameYearPublished>2018</BoardgameYearPublished>
        [XmlElement("BoardgameYearPublished")]
        public int BoardgameYearPublished { get; set; }

    }
}