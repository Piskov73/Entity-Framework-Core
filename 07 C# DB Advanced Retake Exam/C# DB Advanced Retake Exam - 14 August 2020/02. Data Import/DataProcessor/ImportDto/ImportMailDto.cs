using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportMailDto
    {
        //•	Description – text(required)
        [Required]
        public string Description { get; set; } = null!;

        //•	Sender – text(required)
        [Required]
        public string Sender { get; set; } = null!;

        //•	Address – text, consisting only of letters, spaces and numbers
        //, which ends with "str."  (Example: "62 Muir Hill str.")
        [Required]
        [RegularExpression(@"^[A-Za-z0-9 ]+str\.$")]
        public string Address { get; set; } = null!;
    }
}