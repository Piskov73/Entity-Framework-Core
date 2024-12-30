using System.ComponentModel.DataAnnotations;

namespace TeisterMask.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Username – text with length[3, 40]. Should contain only lower or upper case letters and/or digits. (required)
        [Required]
        [MaxLength(40)]
        public string Username { get; set; } = null!;

        //•	Email – text(required). Validate it! There is attribute for this job.
        [Required]
        public string Email { get; set; } = null!;

        //•	Phone – text.Consists only of three groups(separated by '-'),
        //the first two consist of three digits and the last one – of 4 digits. (required)123-123-1234
        [Required]
        [MaxLength(12)]
        public string Phone { get; set; } = null!;

        //•	EmployeesTasks – collection of type EmployeeTask

        public ICollection<EmployeeTask> EmployeesTasks { get; set; }
    }
}
