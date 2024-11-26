namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.Constants;
    public class SellersImportDTO
    {
        //•	Name – text with length[5…20] (required)
        [Required]
        [MinLength(SellerNameMinLength)]
        [MaxLength(SellerNameMaxLength)]
        public string Name { get; set; } = null!;

        //•	Address – text with length[2…30] (required)
        [Required]
        [MinLength(AddressMinLength)]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        //•	Country – text(required)
        [Required]
        public string Country { get; set; } = null!;

        //•	Website – a string (required).
        //First four characters are "www.", followed by upper and lower letters,
        //digits or '-' and the last three characters are ".com".@"(www\.[a-zA-Z0-9\-]{2,256}\.com)"
        [Required]
        [RegularExpression(WebsiteValid)]
        public string Website { get; set; } = null!;

        public List<int> Boardgames { get; set; }=new List<int>();
    }
}
