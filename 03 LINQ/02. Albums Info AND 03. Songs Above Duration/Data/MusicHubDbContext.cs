namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;
    using System;
    using System.Threading.Tasks;

    public class MusicHubDbContext : DbContext
    {
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Performer> Performsers { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }

        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SongPerformer>()
                .HasKey(sp => new { sp.SongId, sp.PerformerId });

        }

        public static implicit operator ValueTask(MusicHubDbContext v)
        {
            throw new NotImplementedException();
        }
    }
}
