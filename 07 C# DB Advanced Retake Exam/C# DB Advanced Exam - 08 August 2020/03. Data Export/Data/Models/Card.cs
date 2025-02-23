using Castle.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        //•	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Number – text, which consists of 4 pairs of 4 digits,
        //separated by spaces(ex. "1234 5678 9012 3456") (required)
        [Required]
        [MaxLength(19)]
        public string Number { get; set; } = null!;

        //•	Cvc – text, which consists of 3 digits(ex. "123") (required)
        [Required]
        [MaxLength(3)]
        public string Cvc { get; set; } = null!;

        //•	Type – enumeration of type CardType, with possible values("Debit", "Credit") (required)
        [Required]
        public CardType Type { get; set; }

        //•	UserId – integer, foreign key(required)
        [Required]
        public int UserId { get; set; }

        //•	User – the card's user (required)
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        //•	Purchases – collection of type Purchase
        public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();



    }
}