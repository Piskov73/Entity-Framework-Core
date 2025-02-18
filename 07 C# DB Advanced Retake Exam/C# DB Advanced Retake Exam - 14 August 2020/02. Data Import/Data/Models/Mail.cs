using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Description – text(required)
        [Required]
        public string Description { get; set; } = null!;

        //•	Sender – text(required)
        [Required]
        public string Sender { get; set; } = null!;

        //•	Address – text, consisting only of letters, spaces and numbers
        //, which ends with "str." (required) (Example: "62 Muir Hill str.")
        [Required]
        public string Address { get; set; } = null!;

        //•	PrisonerId - integer, foreign key(required)
        [Required]
        public int PrisonerId { get; set; }

        //•	Prisoner – the mail's Prisoner (required)
        [Required]
        [ForeignKey(nameof(PrisonerId))]
        public Prisoner Prisoner { get; set; } = null!;

    }
}