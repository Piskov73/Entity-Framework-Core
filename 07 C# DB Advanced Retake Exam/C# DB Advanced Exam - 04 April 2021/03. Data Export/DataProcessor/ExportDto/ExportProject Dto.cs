using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProject_Dto
    {
        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        //<ProjectName>Hyster-Yale</ProjectName>
        [XmlElement("ProjectName")]
        public string ProjectName { get; set; } = null!;

        //<HasEndDate>No</HasEndDate>
        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }= null!;

        [XmlArray("Tasks")]
        [XmlArrayItem("Task")]
        public List<ExportXmlTaskDto> Tasks { get; set; } = new List<ExportXmlTaskDto>();

    }
}
