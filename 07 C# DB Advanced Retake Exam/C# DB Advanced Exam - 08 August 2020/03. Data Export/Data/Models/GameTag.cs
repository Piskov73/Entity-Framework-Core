using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaporStore.Data.Models
{
    public class GameTag
    {
        //•	GameId – integer, Primary Key, foreign key(required)
        [Required]
        public int GameId { get; set; }

        //•	Game – Game
        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = null!;

        //•	TagId – integer, Primary Key, foreign key(required)
        [Required]
        public int TagId { get; set; }

        //•	Tag – Tag
        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; } = null!;

    }
}