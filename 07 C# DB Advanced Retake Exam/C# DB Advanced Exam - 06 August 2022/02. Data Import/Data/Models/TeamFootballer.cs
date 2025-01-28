using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    public class TeamFootballer
    {
        //•	TeamId – integer, Primary Key, foreign key(required)
        [Required]
        public int TeamId { get; set; }

        //•	Team – Team
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; } = null!;

        //•	FootballerId – integer, Primary Key, foreign key(required)
        [Required]
        public int FootballerId { get; set; }

        //•	Footballer – Footballer
        [ForeignKey(nameof(FootballerId))]
        public Footballer Footballer { get; set; } = null!;

    }
}