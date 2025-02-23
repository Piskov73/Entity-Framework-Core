using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportCardDto
    {
        //•	Number – text, which consists of 4 pairs of 4 digits,
        //separated by spaces(ex. "1234 5678 9012 3456") (required)
        [Required]
        [MaxLength(19)]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$")]
        public string Number { get; set; } = null!;

        //•	Cvc – text, which consists of 3 digits(ex. "123") (required)
        [Required]
        [MaxLength(3)]
        [RegularExpression(@"^\d{3}$")]
        public string Cvc { get; set; } = null!;

        //•	Type – enumeration of type CardType, with possible values("Debit", "Credit") (required)
        [Required]
        public string Type { get; set; } = null!;
    }
}