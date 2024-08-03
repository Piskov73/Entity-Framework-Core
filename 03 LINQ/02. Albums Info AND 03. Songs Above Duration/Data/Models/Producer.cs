using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    [Table("Producers")]
    public class Producer
    {
        public Producer()
        {
            this.Albums=new HashSet<Album>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [MaxLength(30)]
        public string? Pseudonym { get; set; }

        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

    }
}
