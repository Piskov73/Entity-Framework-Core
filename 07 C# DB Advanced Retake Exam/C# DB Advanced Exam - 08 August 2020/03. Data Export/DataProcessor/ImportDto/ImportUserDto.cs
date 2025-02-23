using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto
{
    public class ImportUserDto
    {
        //•	Username – text with length[3, 20] (required)
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; } = null!;

        //•	FullName – text, which has two words, consisting of Latin letters.
        //Both start with an upper letter and are followed by lower letters.
        //The two words are separated by a single space (ex. "John Smith") (required)
        [Required]
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; } = null!;

        //•	Email – text(required)
        [Required]
        public string Email { get; set; } = null!;

        //•	Age – integer in the range[3, 103] (required)
        [Required]
        [Range(3,103)]
        public int Age { get; set; }

        //•	Cards – collection of type Card
        public ImportCardDto[] Cards { get; set; } = new ImportCardDto[] { };
    }
}
