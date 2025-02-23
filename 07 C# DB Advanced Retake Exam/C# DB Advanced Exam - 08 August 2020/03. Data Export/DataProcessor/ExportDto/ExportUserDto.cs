using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto
{
    [XmlType("User")]
    public class ExportUserDto
    {
        // <User
        // username = "mgraveson" >
        [XmlAttribute("username")]
        public string Username { get; set; } = null!;
   
        //< Purchases >
        //  < Purchase >
        [XmlArray("Purchases")]
        [XmlArrayItem("Purchase")]
        public ExportPurchaseDto[] Purchases { get; set; } = new ExportPurchaseDto[] { };

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }

    }
}
