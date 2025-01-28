using AutoMapper;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Footballers.Data.Models
{
    public class Team
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[3, 40]. Should contain letters(lower and upper case),
        //digits, spaces, a dot sign('.') and a dash('-'). (required)
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        //•	Nationality – text with length[2, 40] (required)
        [Required]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        //•	Trophies – integer(required)
        [Required]
        public int Trophies { get; set; }

        //•	TeamsFootballers – collection of type TeamFootballer
        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();

    }
}
