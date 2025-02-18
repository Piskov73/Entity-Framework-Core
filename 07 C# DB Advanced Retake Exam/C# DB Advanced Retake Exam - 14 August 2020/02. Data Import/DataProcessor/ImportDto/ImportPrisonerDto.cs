using SoftJail.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerDto
    {
        //•	FullName – text with min length 3 and max length 20 (required)
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; } = null!;

        //•	Nickname – text starting with
        //"The " and a single word only of letters with an uppercase letter for beginning(example: The Prisoner) (required)
        [Required]
        [RegularExpression(@"^The [A-Z][a-z]*$")]
        public string Nickname { get; set; } = null!;

        //•	Age – integer in the range[18, 65] (required)
        [Required]
        [Range(18,65)]
        public int Age { get; set; }

        //•	IncarcerationDate ¬– Date(required)
        [Required]
        public string IncarcerationDate { get; set; } = null!;

        //•	ReleaseDate – Date
        public string? ReleaseDate { get; set; }

        //•	Bail – decimal (non-negative, minimum value: 0)
        [Range(0,double.MaxValue)]
        public decimal? Bail { get; set; }

        //•	CellId - integer, foreign key
        public int? CellId { get; set; }

      

        //•	Mails – collection of type Mail
        public ImportMailDto[] Mails { get; set; } = new ImportMailDto[] { };
    }
}
