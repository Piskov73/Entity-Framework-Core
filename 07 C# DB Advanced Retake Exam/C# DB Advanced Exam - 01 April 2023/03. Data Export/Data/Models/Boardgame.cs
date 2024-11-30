namespace Boardgames.Data.Models
{
    using Boardgames.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Shared.Constants;
    public class Boardgame
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }
        //•	Name – text with length[10…20] (required)
        [Required]
        [StringLength(NameBoardgameMaxLenght)]
        public string Name { get; set; } = null!;

        //•	Rating – double in range[1…10.00] (required)
        [Required]
        public double Rating { get; set; }

        //•	YearPublished – integer in range[2018…2023] (required)
        [Required]
        public int YearPublished { get; set; }

        //•	CategoryType – enumeration of type CategoryType
        //, with possible values(Abstract, Children, Family, Party, Strategy) (required)
        [Required]
        public CategoryType CategoryType { get; set; }

        //•	Mechanics – text(string, not an array) (required)
        [Required]
        public string Mechanics { get; set; }=null!;

        //•	CreatorId – integer, foreign key(required)
        [Required]
        public int CreatorId { get; set; }

        //•	Creator – Creator
        [ForeignKey(nameof(CreatorId))]
        public Creator Creator { get; set; } = null!;

        //•	BoardgamesSellers – collection of type BoardgameSeller

        public ICollection<BoardgameSeller> BoardgamesSellers { get; set; }=new HashSet<BoardgameSeller>();
    }
}
