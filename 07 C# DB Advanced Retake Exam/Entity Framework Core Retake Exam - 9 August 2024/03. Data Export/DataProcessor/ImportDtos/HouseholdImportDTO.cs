namespace NetPay.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using static Shared.Constants;

    [XmlType("Household")]
    public class HouseholdImportDTO
    {
        //•	PhoneNumber – text with length 15. (required)
        //o   The phone number must start with a plus sign, followed by exactly three digits for the country code, a slash, exactly three digits for the area or service provider code, a dash, and exactly six digits for the subscriber number: 
        //	Example -> +144/123-123456 
        //	Use the following string for correct validation: @"^\+\d{3}/\d{3}-\d{6}$"
        [XmlAttribute("phone")]
        [Required]
        [MinLength(PhoneNumberMinLength)]
        [MaxLength(PhoneNumberMaxLength)]
        [RegularExpression(PhoneNumberValidation)]

        public string PhoneNumber { get; set; } = null!;
        //•	ContactPerson - text with length[5, 50] (required)
        [XmlElement("ContactPerson")]
        [Required]
        [MinLength(ContactPersonMinLength)]
        [MaxLength(ContactPersonMaxLength)]
        public string ContactPerson { get; set; } = null!;

        //•	Email – text with length[6, 80] (not required)
        [XmlElement("Email")]
        [MinLength(EmailMinLegngth)]
        [MaxLength(EmailMaxLegngth)]
        public string? Email { get; set; }




    }
}
