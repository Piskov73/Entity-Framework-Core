using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        //•	Name – text with length[5…20] (required)
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        //•	Address – text with length[2…30] (required)
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Address { get; set; } = null!;

        //•	Country – text(required)
        [Required]
        public string Country { get; set; } = null!;

        //•	Website – a string (required).
        //First four characters are "www.", followed by upper and lower letters,
        //digits or '-' and the last three characters are ".com".
        [Required]
        [RegularExpression(@"^www\.[a-zA-Z0-9-]+\.com$")]
        public string Website { get; set; } = null!;

        //•	BoardgamesSellers – collection of type BoardgameSeller

        public List<int> Boardgames { get; set; } = new List<int>();
    }
}
