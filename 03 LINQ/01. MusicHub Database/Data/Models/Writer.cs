using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    [Table("Writers")]
    public class Writer
    {
        public Writer()
        {
            this.Songs=new HashSet<Song>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string? Pseudonym { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

    }
}
