using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    [Table("PlayerStatistic")]
    public class PlayerStatistic
    {
        // GameId, PlayerId, ScoredGoals, Assists, MinutesPlayed

        public int GameId { get; set; }
        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; }
        public int PlayerId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public virtual Player Player { get; set; }

        public byte ScoredGoals { get; set; }

        public byte Assists { get; set; }

        public byte MinutesPlayed { get; set; }
    }
}
