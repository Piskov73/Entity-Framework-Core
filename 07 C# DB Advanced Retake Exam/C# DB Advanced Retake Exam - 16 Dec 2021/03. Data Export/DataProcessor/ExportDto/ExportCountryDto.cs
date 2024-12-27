using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Country")]
    public class ExportCountryDto
    {
        // <Country Country="Sweden"
        [XmlAttribute("Country")]
        public string Country { get; set; } = null!;

        // ArmySize="5437337" />
        [XmlAttribute("ArmySize")]
        public int ArmySize { get; set; }
    }
}