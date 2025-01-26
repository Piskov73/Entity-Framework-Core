using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Truck")]
    public class ExportTruckDto
    {
        //<Truck>
        //<RegistrationNumber>CT5203MM</RegistrationNumber>
        [XmlElement("RegistrationNumber")]
        public string RegistrationNumber { get; set; } = null!;

        //<Make>Mercedes</Make>
        [XmlElement("Make")]
        public string Make { get; set; } = null!;

    }
}