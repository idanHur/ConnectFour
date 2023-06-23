using Connect4Game;
using GameManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Player = GameManager.Models.Player;

namespace GameManager.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Games) 
                .WithOne()
                .HasForeignKey(g => g.playerId);

            modelBuilder.Entity<Game>()
                .HasOne<Player>()
                .WithMany(p => p.Games) 
                .HasForeignKey(g => g.playerId);
        }

    }
}
