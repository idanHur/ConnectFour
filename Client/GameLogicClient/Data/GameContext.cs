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
                .HasKey(p => p.playerId); // Set playerId as primary key
            modelBuilder.Entity<Player>()
                .Property(p => p.playerId)
                .ValueGeneratedNever(); // Disable auto incrementation

            modelBuilder.Entity<Game>()
                .HasKey(g => g.gameId); // Set gameId as primary key
            modelBuilder.Entity<Game>()
                .Property(g => g.gameId)
                .ValueGeneratedNever(); // Disable auto incrementation
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Player)
                .WithMany(p => p.games)
                .HasForeignKey(g => g.PlayerId); // Set PlayerId as foreign key

            modelBuilder.Entity<Move>()
                .HasKey(m => m.id); // Set id as primary key
            modelBuilder.Entity<Move>()
                .Property(m => m.id)
                .ValueGeneratedNever(); // Disable auto incrementation
            modelBuilder.Entity<Move>()
                .HasOne(m => m.Game)
                .WithMany(g => g.moves)
                .HasForeignKey(m => m.GameId) // Set GameId as foreign key
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
