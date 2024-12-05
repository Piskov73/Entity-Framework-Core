namespace TravelAgency.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Shared.ValidatingConstants;

    [XmlType("Customer")]
    public class CustomerImportDTO
    {
     

        //•	FullName – text with length[4, 60] (required)
        [XmlElement("FullName")]
        [Required]
        [MaxLength(CustomerNameMaxLehgth)]
        [MinLength(CustomerNameMinLehgth)]
        public string FullName { get; set; } = null!;

        //•	Email – text with length[6, 50] (required)
        [XmlElement("Email")]
        [Required]
        [MaxLength(EmailMaxLength)]
        [MinLength(EmailMinLength)]
        public string Email { get; set; } = null!;
        //•	PhoneNumber – text with length 13. (required)
        //o   All phone numbers must have the following structure: a plus sign followed by 12 digits, without spaces or special characters: 
        //	Example -> +359888555444 
        //	HINT -> use DataAnnotation[RegularExpression] 
        [XmlAttribute("phoneNumber")]
        [Required]
        [RegularExpression(ValidPhoneNumberPattern)]
        public string PhoneNumber { get; set; } = null!;


    }
}
