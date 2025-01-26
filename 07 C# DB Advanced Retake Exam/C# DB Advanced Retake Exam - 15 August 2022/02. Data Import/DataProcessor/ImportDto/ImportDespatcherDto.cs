using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        //•	Name – text with length[2, 40] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	Position – text
        [XmlElement("Position")]
        [Required]
        public string Position { get; set; } = null!;

        //•	Trucks – collection of type Truck
        [XmlArray("Trucks")]
        [XmlArrayItem("Truck")]
        public List<ImportTruckDto> Trucks { get; set; } = new List<ImportTruckDto>();
    }
}
