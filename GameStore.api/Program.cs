using GameStore.api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
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



// Get all games
app.MapGet("/games", () => games);

const string GetGameEndpointName = "GetGame";

// Get specific game by ID
app.MapGet("/games/{id}", (int id) => {
    var game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName); 

// Post a Game(Create)
app.MapPost("games", (CreateGameDto newGame) =>
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
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
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
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});
app.Run();
