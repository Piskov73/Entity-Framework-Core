using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Task")]
    public class ExportXmlTaskDto
    {
        //<Task>

        //<Name>Broadleaf</Name>
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        //<Label>JavaAdvanced</Label>
        [XmlElement("Label")]
        public string Label { get; set; } = null!;


    }
}