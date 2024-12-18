using System;
using GameStore.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.Entity<Genre>().HasData(
                new {Id = 1, Name = "Fighting"},
                new {Id = 2, Name = "Adventure"},
                new {Id = 3, Name = "Racing"},
                new {Id = 4, Name = "Puzzle"},
                new {Id = 5, Name = "Strategy"},
                new {Id = 6, Name = "Simulation"}

            );
    }

}
