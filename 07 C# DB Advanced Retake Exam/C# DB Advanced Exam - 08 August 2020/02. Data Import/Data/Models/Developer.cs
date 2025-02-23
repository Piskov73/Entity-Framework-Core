using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Developer
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text(required)
        [Required]
        public string Name { get; set; } = null!;

        //•	Games - collection of type Game
        public ICollection<Game> Games { get; set; } = new HashSet<Game>();

    }
}