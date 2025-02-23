using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models
{
    public class Game
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text(required)
        [Required]
        public string Name { get; set; } = null!;

        //•	Price – decimal (non-negative, minimum value: 0) (required)
        [Required]
        public decimal Price { get; set; }

        //•	ReleaseDate – Date(required)
        [Required]
        public DateTime ReleaseDate { get; set; }

        //•	DeveloperId – integer, foreign key(required)
        [Required]
        public int DeveloperId { get; set; }

        //•	Developer – the game's developer (required)
        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; } = null!;

        //•	GenreId – integer, foreign key(required)
        [Required]
        public int GenreId { get; set; }

        //•	Genre – the game's genre (required)
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        //•	Purchases - collection of type Purchase
        public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

        //•	GameTags - collection of type GameTag.Each game must have at least one tag.
        public ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();

    }
}
