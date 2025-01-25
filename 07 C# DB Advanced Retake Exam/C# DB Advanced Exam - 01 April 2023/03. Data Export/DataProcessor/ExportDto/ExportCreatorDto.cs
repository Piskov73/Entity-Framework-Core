using Boardgames.Data.Models;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        //<Creator BoardgamesCount = "6" >
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        //< CreatorName > Cade O'Neill</CreatorName>
        [XmlElement("CreatorName")]
        public string CreatorName { get; set; } = null!;

        //<Boardgames>
        //<Boardgame>
        [XmlArray("Boardgames")]
        [XmlArrayItem("Boardgame")]
        public List<ExportBoardgameDto> Boardgames { get; set; } = new List<ExportBoardgameDto>();

    }
}
