using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Car")]
    public class CarImportDTO
    {
        public CarImportDTO()
        {
            Parts = new List<PartIdDTO>();
        }
        [XmlElement("make")]
        public string Make { get; set; } = null!;

        [XmlElement("model")]
        public string Model { get; set; } = null!;

        [XmlElement("traveledDistance")]
        public long TraveledDistance { get; set; }
        [XmlArray("parts")]
        [XmlArrayItem("partId")]
        public List<PartIdDTO> Parts { get; set; }

    }

    [XmlType("partId")]
    public class PartIdDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
