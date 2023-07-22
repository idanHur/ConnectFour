using GameLogicClient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicClient.Data
{
    public class GameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Move> Moves { get; set; }

        public GameContext(DbContextOptions<GameContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasKey(p => p.PlayerId); // Set PlayerId as primary key
            modelBuilder.Entity<Player>()
                .Property(p => p.PlayerId)
                .ValueGeneratedNever(); // Disable auto incrementation

            modelBuilder.Entity<Game>()
                .HasKey(g => g.GameId); // Set GameId as primary key
            modelBuilder.Entity<Game>()
                .Property(g => g.GameId)
                .ValueGeneratedNever(); // Disable auto incrementation
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Player)
                .WithMany(p => p.Games)
                .HasForeignKey(g => g.PlayerId); // Set PlayerId as foreign key

            modelBuilder.Entity<Move>()
                .HasKey(m => m.Id); // Set Id as primary key
            modelBuilder.Entity<Move>()
                .Property(m => m.Id)
                .ValueGeneratedNever(); // Disable auto incrementation
            modelBuilder.Entity<Move>()
                .HasOne(m => m.Game)
                .WithMany(g => g.Moves)
                .HasForeignKey(m => m.GameId) // Set GameId as foreign key
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
