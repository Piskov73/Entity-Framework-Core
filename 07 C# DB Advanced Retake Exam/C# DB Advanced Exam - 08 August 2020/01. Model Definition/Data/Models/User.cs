using AutoMapper;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class User
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Username – text with length[3, 20] (required)
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; } = null!;

        //•	FullName – text, which has two words, consisting of Latin letters.
        //Both start with an upper letter and are followed by lower letters.
        //The two words are separated by a single space (ex. "John Smith") (required)
        [Required]
        public string FullName { get; set; } = null!;

        //•	Email – text(required)
        [Required]
        public string Email { get; set; } = null!;

        //•	Age – integer in the range[3, 103] (required)
        [Required]
        public int Age { get; set; }

        //•	Cards – collection of type Card
        public ICollection<Card> Cards { get; set; } = new HashSet<Card>();

    }
}
