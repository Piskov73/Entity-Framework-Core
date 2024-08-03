using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    [Table("SongsPerformers")]
    public class SongPerformer
    {
        [ForeignKey(nameof(Song))]
        public int SongId { get; set; }
        [Required]
        public Song Song { get; set; } = null!;


        public int PerformerId { get; set; }
        [Required]
        [ForeignKey(nameof(PerformerId))]
        public Performer Performer { get; set; } = null!;
    }
}
