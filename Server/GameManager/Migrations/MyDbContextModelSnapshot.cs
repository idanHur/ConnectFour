﻿// <auto-generated />
using System;
using GameManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameManager.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Connect4Game.Game", b =>
                {
                    b.Property<int>("gameId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("currentPlayer");

                    b.Property<TimeSpan>("gameDuration");

                    b.Property<int>("gameStatus");

                    b.Property<int>("playerId");

                    b.Property<DateTime>("startTime");

                    b.HasKey("gameId");

                    b.HasIndex("playerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Connect4Game.Move", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Player");

                    b.Property<int>("columnNumber");

                    b.Property<int?>("gameId");

                    b.HasKey("Id");

                    b.HasIndex("gameId");

                    b.ToTable("Move");
                });

            modelBuilder.Entity("GameManager.Models.Player", b =>
                {
                    b.Property<int>("playerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("country")
                        .IsRequired();

                    b.Property<string>("password")
                        .IsRequired();

                    b.Property<string>("phoneNumber")
                        .IsRequired();

                    b.Property<string>("playerName")
                        .IsRequired();

                    b.HasKey("playerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Connect4Game.Game", b =>
                {
                    b.HasOne("GameManager.Models.Player")
                        .WithMany("games")
                        .HasForeignKey("playerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Connect4Game.Move", b =>
                {
                    b.HasOne("Connect4Game.Game")
                        .WithMany("Moves")
                        .HasForeignKey("gameId");
                });
#pragma warning restore 612, 618
        }
    }
}
