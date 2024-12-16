using System.Security.Cryptography;
using GameStore.api.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetGameEndPoint = "GetGame";

List<GameDto> games = [
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

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id ==id))
    .WithName(GetGameEndPoint);


// POST /games
app.MapPost("games", (CreateGameDto newGame) => {
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate);

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndPoint, new {id = game.Id}, game);
} );


app.Run();
