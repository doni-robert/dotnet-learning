using System;
using GameStore.api.Dtos;

namespace GameStore.api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndPoint = "GetGame";

    private static readonly List<GameDto> games = [
        new(1, "The Witcher 3: Wild Hunt", "RPG", 29.99m, new DateOnly(2015, 5, 19)),
        new(2, "The Legend of Zelda: Breath of the Wild", "Adventure", 59.99m, new DateOnly(2017, 3, 3)),
        new(3, "Red Dead Redemption 2", "Action-Adventure", 39.99m, new DateOnly(2018, 10, 26)),
        new(4, "Cyberpunk 2077", "RPG", 49.99m, new DateOnly(2020, 12, 10)),
        new(5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18)),
        new(6, "Grand Theft Auto V", "Action-Adventure", 29.99m, new DateOnly(2013, 9, 17)),
        new(7, "Fortnite", "Battle Royale", 0.00m, new DateOnly(2017, 7, 21)),
        new(8, "Among Us", "Party", 4.99m, new DateOnly(2018, 6, 15)),
        new(9, "Call of Duty: Modern Warfare", "Shooter", 59.99m, new DateOnly(2019, 10, 25)),
        new(10, "Elden Ring", "RPG", 59.99m, new DateOnly(2022, 2, 25))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {  
        var group = app.MapGroup("games")
                        .WithParameterValidation();

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) => 
        {
            var game= games.Find(game => game.Id ==id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndPoint);


        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndPoint, new {id = game.Id}, game);
        } );

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1) 
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        } );

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) => {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }


}
