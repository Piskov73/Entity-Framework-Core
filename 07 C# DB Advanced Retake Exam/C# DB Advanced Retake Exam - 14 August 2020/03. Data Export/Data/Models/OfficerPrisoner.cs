using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        //•	PrisonerId – integer, Primary Key
        [Required]
        public int PrisonerId { get; set; }

        //•	Prisoner – the officer's prisoner (required)
        [Required]
        [ForeignKey(nameof(PrisonerId))]
        public Prisoner Prisoner { get; set; } = null!;

        //•	OfficerId – integer, Primary Key
        [Required]
        public int  OfficerId { get; set; }

        //•	Officer – the prisoner's officer (required)
        [Required]
        [ForeignKey(nameof(OfficerId))]
        public Officer Officer { get; set; } = null!;

    }
}