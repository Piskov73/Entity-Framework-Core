using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("Purchase")]
    public class ExportPurchaseDto
    {
        // <Purchase>

        //<Card>7991 7779 5123 9211</Card>
        [XmlElement("Card")]
        public string Card { get; set; } = null!;

        //<Cvc>340</Cvc>
        [XmlElement("Cvc")]
        public string Cvc { get; set; } = null!;

        //<Date>2017-08-31 17:09</Date>
        [XmlElement("Date")]
        public string Date { get; set; } = null!;

        //<Game title = "Counter-Strike: Global Offensive" >
        [XmlElement("Game")]

        public ExportGameDto Game { get; set; } = null!;

    }
}