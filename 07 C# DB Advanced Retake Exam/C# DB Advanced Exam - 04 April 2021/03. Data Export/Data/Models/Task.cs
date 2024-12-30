using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeisterMask.Data.Models.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace TeisterMask.Data.Models
{
    public class Task
    {
        public Task()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[2, 40] (required)
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	OpenDate – date and time(required)
        [Required]
        public DateTime OpenDate { get; set; }

        //•	DueDate – date and time(required)
        [Required]
        public DateTime DueDate { get; set; }

        //•	ExecutionType – enumeration of type ExecutionType,
        //with possible values(ProductBacklog, SprintBacklog, InProgress, Finished) (required)
        [Required]
        public ExecutionType ExecutionType { get; set; }


        //•	LabelType – enumeration of type LabelType,
        //with possible values(Priority, CSharpAdvanced, JavaAdvanced, EntityFramework, Hibernate) (required)
        [Required]
        public LabelType LabelType { get; set; }

        //•	ProjectId – integer, foreign key(required)
        [Required]
        public int ProjectId { get; set; }

        //•	Project – Project 
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; } = null!;

        //•	EmployeesTasks – collection of type EmployeeTask
        public ICollection<EmployeeTask> EmployeesTasks { get; set; }

    }
}
