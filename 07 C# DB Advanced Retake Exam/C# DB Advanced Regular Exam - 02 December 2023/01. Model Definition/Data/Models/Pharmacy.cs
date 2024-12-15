using System.ComponentModel.DataAnnotations;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[2, 50] (required)

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        //•	PhoneNumber – text with length 14. (required)
        //o   All phone numbers must have the following structure: three digits enclosed in parentheses, followed by a space, three more digits, a hyphen, and four final digits: 
        //	Example -> (123) 456-7890 

        [Required]
        [StringLength(14)]
        public string PhoneNumber { get; set; } = null!;

        //•	IsNonStop – bool (required)

        [Required]
        public bool IsNonStop { get; set; }

        //•	Medicines - collection of type Medicine

        public ICollection<Medicine> Medicines { get; set; }= new HashSet<Medicine>();

    }
}
