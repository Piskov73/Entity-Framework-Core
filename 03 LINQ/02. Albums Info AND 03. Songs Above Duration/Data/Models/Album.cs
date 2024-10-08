﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    [Table("Albums")]
    public class Album
    {
        public Album()
        {
            this.Songs=new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime ReleaseDate { get; set; }

      
        public decimal Price =>this.Songs.Sum(s => s.Price);

        public int? ProducerId { get; set; }

        [ForeignKey(nameof(ProducerId))]
        public Producer Producer { get; set; }

        
        public virtual ICollection<Song> Songs { get; set; }

    }
}
