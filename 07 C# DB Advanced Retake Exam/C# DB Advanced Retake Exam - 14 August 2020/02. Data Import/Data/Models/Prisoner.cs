using static System.Net.Mime.MediaTypeNames;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set;  }

        //•	FullName – text with min length 3 and max length 20 (required)
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; } = null!;

        //•	Nickname – text starting with
        //"The " and a single word only of letters with an uppercase letter for beginning(example: The Prisoner) (required)
        [Required]
        public string Nickname { get; set; } = null!;

        //•	Age – integer in the range[18, 65] (required)
        [Required]
        public int Age { get; set; }

        //•	IncarcerationDate ¬– Date(required)
        [Required]
        public DateTime IncarcerationDate { get; set; }

        //•	ReleaseDate – Date
        public DateTime? ReleaseDate { get; set; }

        //•	Bail – decimal (non-negative, minimum value: 0)
        public decimal? Bail { get; set; }

        //•	CellId - integer, foreign key
        public int? CellId { get; set; }

        //•	Cell – the prisoner's cell
        [ForeignKey(nameof(CellId))]
        public Cell? Cell { get; set; }

        //•	Mails – collection of type Mail
        public ICollection<Mail> Mails { get; set; } = new HashSet<Mail>();

        //•	PrisonerOfficers - collection of type OfficerPrisoner
        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; } = new HashSet<OfficerPrisoner>();

    }
}
