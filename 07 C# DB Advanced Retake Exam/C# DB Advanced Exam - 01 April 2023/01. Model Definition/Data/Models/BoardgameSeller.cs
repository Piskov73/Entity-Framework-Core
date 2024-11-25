using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models
{
    public class BoardgameSeller
    {
        //•	BoardgameId – integer, Primary Key, foreign key(required)
        public int BoardgameId { get; set; }
        //•	Boardgame – Boardgame
        [ForeignKey(nameof(BoardgameId))]
        public Boardgame Boardgame { get; set; } = null!;

        //•	SellerId – integer, Primary Key, foreign key(required)
        public int SellerId { get; set; }

        //•	Seller – Seller
        [ForeignKey(nameof(SellerId))]
        public Seller Seller { get; set; } = null!;
    }
}
