using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
new (1, "GTA V",
    "Action",
    45.99M,
    new DateOnly(2003,9,23)),
new (2, "Watch Dogs",
    "Action-Adventure",
    21.99M,
    new DateOnly(1998,6,3)),
new (3, "Need For Speed: Underground 2",
    "Racing",
    39.99M,
    new DateOnly(2004,9,2)),
];
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        // Get all games
        group.MapGet("/", () => games);


        // Get specific game by ID
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        // Post a Game(Create)
        group.MapPost("/games", (CreateGameDto newGame) =>
        {
            GameDto game = new(
            games.Count + 1,
            newGame.Name,
            newGame.Genre,
            newGame.Price,
            newGame.ReleaseDate
            );
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // Update a Game(PUT)
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
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
            updatedGame.ReleaseDate);

            return Results.NoContent();
        });

        // Delete a Game
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
    }
}
