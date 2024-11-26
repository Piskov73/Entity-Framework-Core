namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.Constants;
    public class Creator
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	FirstName – text with length[2, 7] (required) 
        [Required]
        [StringLength(CreatorFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        //•	LastName – text with length[2, 7] (required)
        [Required]
        [StringLength(CreatorLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        //•	Boardgames – collection of type Boardgame
        public HashSet<Boardgame> Boardgames { get; set; } = new HashSet<Boardgame>();

    }
}
