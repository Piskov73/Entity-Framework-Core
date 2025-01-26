using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespatcherDto
    {
        //     <Despatcher TrucksCount = "6" >
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }

        //< DespatcherName > Vladimir Hristov</DespatcherName>
        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; } = null!;

        //<Trucks>
        //  <Truck>
        [XmlArray("Trucks")]
        [XmlArrayItem("Truck")]
        public List<ExportTruckDto> Trucks { get; set; } = new List<ExportTruckDto>();


    }
}
