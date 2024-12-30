using System.ComponentModel.DataAnnotations;

namespace TeisterMask.Data.Models
{
    public class Project
    {
        public Project()
        {
            this.Tasks=new HashSet<Task>(); 
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

        //•	DueDate – date and time(can be null)
        public DateTime? DueDate { get; set; }

        //•	Tasks – collection of type Task

        public ICollection<Task> Tasks { get; set; }
    }
}
