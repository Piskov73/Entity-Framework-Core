using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        //•	EmployeeId – integer, Primary Key, foreign key(required)
        [Required]
        public int EmployeeId { get; set; }

        //•	Employee – Employee
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; } = null!;

        //•	TaskId – integer, Primary Key, foreign key(required)
        [Required]
        public int TaskId { get; set; }

        //•	Task – Task
        [ForeignKey(nameof(TaskId))]
        public Task Task { get; set; } = null!;

    }
}