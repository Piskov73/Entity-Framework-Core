using System.Diagnostics;
using System.Xml.Serialization;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("Game")]
    public class ExportGameDto
    {
        //<Game

        //title = "Counter-Strike: Global Offensive" >
        [XmlAttribute("title")]
        public string GameName { get; set; } = null!;

        //  < Genre > Action </ Genre >
        [XmlElement("Genre")]
        public string Genre { get; set; } = null!;

        //  < Price > 12.49 </ Price >
        [XmlElement("Price")]
        public decimal Price { get; set; }

    }
}