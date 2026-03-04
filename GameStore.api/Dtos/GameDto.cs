using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record GameDto(
    int Id,
    [Required][StringLength(50)] string Name,
    [Required][StringLength(30)] string Genre,
    [Range(1, 1000)] decimal Price,
    DateOnly ReleaseDate);