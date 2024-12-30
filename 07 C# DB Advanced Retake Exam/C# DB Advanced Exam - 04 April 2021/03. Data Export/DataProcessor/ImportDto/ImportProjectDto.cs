using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjectDto
    {
        //•	Name – text with length[2, 40] (required)
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	OpenDate – date and time(required)
        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }=null!;

        //•	DueDate – date and time(can be null)
        [XmlElement("DueDate")]
        public string? DueDate { get; set; }

        //•	Tasks – collection of type Task
        [XmlArray("Tasks")]
        [XmlArrayItem("Task")]
        public List<ImportTaskDto> Tasks { get; set; } =new List<ImportTaskDto>();
    }
}
