﻿using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    [Table("Games")]
    public class Game
    {

        public Game()
        {
            this.PlayersStatistics=new HashSet<PlayerStatistic>();
            this.Bets=new HashSet<Bet>();
        }
        [Key]
        public int GameId { get; set; }

        public int HomeTeamId { get; set; }

        [ForeignKey(nameof(HomeTeamId))]
        public virtual Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        [ForeignKey(nameof(AwayTeamId))]
        public virtual Team AwayTeam { get; set; }
        public byte HomeTeamGoals { get; set; }

        public byte AwayTeamGoals { get; set; }

        public decimal HomeTeamBetRate { get; set; }

        public decimal AwayTeamBetRate { get; set; }

        public decimal DrawBetRate { get; set; }

        public DateTime DateTime { get; set; }


        [MaxLength(ValidationConstants.GameResultMaxLength)]
        public string? Result { get; set; } 

        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
