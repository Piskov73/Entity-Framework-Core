
namespace NetPay.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.Constants;
    public class Household
    {
        //        •	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	ContactPerson - text with length[5, 50] (required)
        [Required]
        [StringLength(ContactPersonMaxLength)]
        public string ContactPerson { get; set; } = null!;

        //•	Email – text with length[6, 80] (not required)
        [StringLength(EmailMaxLegngth)]
        public string? Email { get; set; }

        //•	PhoneNumber – text with length 15. (required)
        //o   The phone number must start with a plus sign, followed by exactly three digits for the country code, a slash, exactly three digits for the area or service provider code, a dash, and exactly six digits for the subscriber number: 
        //	Example -> +144/123-123456 
        //	Use the following string for correct validation: @"^\+\d{3}/\d{3}-\d{6}$"
        [Required]
        [StringLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;
       
        //•	Expenses - a collection of type Expense
        public ICollection<Expense> Expenses { get; set; }=new HashSet<Expense>();

    }
}
