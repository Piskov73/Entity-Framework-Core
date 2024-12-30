using System.ComponentModel.DataAnnotations;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeesDto
    {
        //•	Username – text with length[3, 40]. Should contain only lower or upper case letters and/or digits. (required)
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string Username { get; set; } = null!;

        //•	Email – text(required). Validate it! There is attribute for this job.
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        //•	Phone – text.Consists only of three groups(separated by '-'),
        //the first two consist of three digits and the last one – of 4 digits. (required)123-123-1234
        [Required]
        [MaxLength(12)]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
        public string Phone { get; set; } = null!;

        //•	EmployeesTasks – collection of type EmployeeTask

        public int[]? Tasks { get; set; }
    }
}
