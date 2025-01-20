using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Medicine")]
    public class ExportMedicineDto
    {
        //  <Medicine Category = "antibiotic" >
        [XmlAttribute("Category")]
        public string Category { get; set; } = null!;

        //  < Name > Aleve(Naproxen) </ Name >
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        //  < Price > 10.50 </ Price >
        [XmlElement("Price")]
        public string Price { get; set; } = null!;

        //  < Producer > HealthCare Pharma</Producer>
        [XmlElement("Producer")]
        public string Producer { get; set; } = null!; 

        //  <BestBefore>2025-09-01</BestBefore>
        public string BestBefore { get; set; } = null!;

     

    }
}