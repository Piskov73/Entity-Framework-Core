using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Tag
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text(required)
        [Required]
        public string Name { get; set; } = null!;

        //•	GameTags – collection of type GameTag
        public ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();

    }
}
