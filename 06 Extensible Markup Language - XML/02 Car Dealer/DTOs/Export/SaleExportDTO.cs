using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("sale")]
    public class SaleExportDTO
    {
        [XmlElement("car")]
        public CarDTO Car { get; set; } = null!;
        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; } = null!;
        [XmlElement("price")]
        public decimal Price {  get; set; }

        [XmlElement("price-with-discount")]
        public double PriceWithDiscount { get; set; } 
    }

    [XmlType("car")]
    public class CarDTO
    {
        [XmlAttribute("make")]
        public string Make { get; set; } = null!;
        [XmlAttribute("model")]
        public string Model { get; set; } = null!;
        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }
    }
}
