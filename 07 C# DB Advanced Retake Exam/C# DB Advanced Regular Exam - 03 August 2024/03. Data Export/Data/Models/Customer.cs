namespace TravelAgency.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.ValidatingConstants;
    public class Customer
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	FullName – text with length[4, 60] (required)
        [Required]
        [StringLength(CustomerNameMaxLehgth)]
        public string FullName { get; set; } = null!;

        //•	Email – text with length[6, 50] (required)
        [Required]
        [StringLength(EmailMaxLength)]
        public string Email { get; set; } = null!;
        //•	PhoneNumber – text with length 13. (required)
        //o   All phone numbers must have the following structure: a plus sign followed by 12 digits, without spaces or special characters: 
        //	Example -> +359888555444 
        //	HINT -> use DataAnnotation[RegularExpression] 
        [Required]
        [StringLength(PhoneNumberLegnth)]
        public string PhoneNumber { get; set; } = null!;

        //•	Bookings - a collection of type Booking
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

    }
}
