using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportTaskDto
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

        //•	DueDate – date and time(required)
        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }= null!;

        //•	ExecutionType – enumeration of type ExecutionType,
        //with possible values(ProductBacklog, SprintBacklog, InProgress, Finished) (required)
        [XmlElement("ExecutionType")]
        [Required]
        [Range(0,3)]
        public int ExecutionType { get; set; }

        //•	LabelType – enumeration of type LabelType,
        //with possible values(Priority, CSharpAdvanced, JavaAdvanced, EntityFramework, Hibernate) (required)
        [XmlElement("LabelType")]
        [Required]
        [Range(0,4)]
        public int LabelType { get; set; }
    }
}