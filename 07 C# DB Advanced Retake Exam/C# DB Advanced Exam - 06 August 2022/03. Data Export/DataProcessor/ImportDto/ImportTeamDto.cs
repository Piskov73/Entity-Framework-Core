using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        //•	Name – text with length[3, 40]. Should contain letters(lower and upper case),
        //digits, spaces, a dot sign('.') and a dash('-'). (required)
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"^[a-zA-Z0-9 .-]+$")]
        public string Name { get; set; } = null!;

        //•	Nationality – text with length[2, 40] (required)
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        //•	Trophies – integer(required)
        [Required]
        [Range(1,int.MaxValue)]
        public int Trophies { get; set; }

        //•	TeamsFootballers – collection of type TeamFootballer
        public List<int> Footballers { get; set; } = new List<int>();
    }
}
