using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    public class Footballer
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[2, 40] (required)
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	ContractStartDate – date and time(required)
        [Required]
        public DateTime ContractStartDate { get; set; }

        //•	ContractEndDate – date and time(required)
        [Required]
        public DateTime ContractEndDate { get; set; }

        //•	PositionType - enumeration of type PositionType, with possible
        //values(Goalkeeper, Defender, Midfielder, Forward) (required)
        [Required]
        public PositionType PositionType { get; set; }

        //•	BestSkillType – enumeration of type BestSkillType, with possible
        //values(Defence, Dribble, Pass, Shoot, Speed) (required)
        [Required]
        public BestSkillType BestSkillType { get; set; }

        //•	CoachId – integer, foreign key(required)
        [Required]
        public int CoachId { get; set; }

        //•	Coach – Coach 
        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; } = null!;

        //•	TeamsFootballers – collection of type TeamFootballer
        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();

    }
}
