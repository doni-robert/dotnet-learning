using System;
using GameStore.api.Data;
using GameStore.api.Dtos;
using GameStore.api.Entities;
using GameStore.api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndPoint = "GetGame";

    // private static readonly List<GameSummaryDto> games = [
    //     new(1, "The Witcher 3: Wild Hunt", "RPG", 29.99m, new DateOnly(2015, 5, 19)),
    //     new(2, "The Legend of Zelda: Breath of the Wild", "Adventure", 59.99m, new DateOnly(2017, 3, 3)),
    //     new(3, "Red Dead Redemption 2", "Action-Adventure", 39.99m, new DateOnly(2018, 10, 26)),
    //     new(4, "Cyberpunk 2077", "RPG", 49.99m, new DateOnly(2020, 12, 10)),
    //     new(5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18)),
    //     new(6, "Grand Theft Auto V", "Action-Adventure", 29.99m, new DateOnly(2013, 9, 17)),
    //     new(7, "Fortnite", "Battle Royale", 0.00m, new DateOnly(2017, 7, 21)),
    //     new(8, "Among Us", "Party", 4.99m, new DateOnly(2018, 6, 15)),
    //     new(9, "Call of Duty: Modern Warfare", "Shooter", 59.99m, new DateOnly(2019, 10, 25)),
    //     new(10, "Elden Ring", "RPG", 59.99m, new DateOnly(2022, 2, 25))
    // ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {  
        var group = app.MapGroup("games")
                        .WithParameterValidation();

        // GET /games
        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking());

        // GET /games/1
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) => 
        {
            Game? game= dbContext.Games.Find(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        })
        .WithName(GetGameEndPoint);


        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
                        
            dbContext.Games.Add(game);
            dbContext.SaveChanges();           

            return Results.CreatedAtRoute(GetGameEndPoint, new {id = game.Id}, game.ToGameDetailsDto());
        } );

        // PUT /games
        group.MapPut("/{id}", (int id, GameStoreContext dbContext, UpdateGameDto updatedGame) => {
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null) 
            {
                return Results.NotFound();
            }
            
            dbContext.Entry(existingGame)
                        .CurrentValues
                        .SetValues(updatedGame.ToEntity(id));
            
            dbContext.SaveChanges();

            return Results.NoContent();
        } );

        // DELETE /games/1
        group.MapDelete("/{id}", (GameStoreContext dbContext, int id) => {
            games.RemoveAll(game => game.Id == id);

            dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDelete();

            return Results.NoContent();
        });

        return group;
    }


}
