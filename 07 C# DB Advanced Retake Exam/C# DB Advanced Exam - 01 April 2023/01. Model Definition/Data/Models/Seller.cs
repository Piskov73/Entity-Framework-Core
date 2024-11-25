namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Shared.Constants;
    public class Seller
    {
        //        •	Id – integer, Primary Key
        [Key]
        public int Id { get; set; }

        //•	Name – text with length[5…20] (required)
        [Required]
        [StringLength(SellerNameMaxLength)]
        public string Name { get; set; } = null!;

        //•	Address – text with length[2…30] (required)
        [Required]
        [StringLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        //•	Country – text(required)
        [Required]
        public string Country { get; set; } = null!;

        //•	Website – a string (required).
        //First four characters are "www.", followed by upper and lower letters,
        //digits or '-' and the last three characters are ".com".
        [Required]
        public string Website { get; set; } = null!;

        //•	BoardgamesSellers – collection of type BoardgameSeller
        public ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new HashSet<BoardgameSeller>();

    }
}
