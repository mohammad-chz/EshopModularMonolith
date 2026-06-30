namespace Catalog.Dtos;

public record ProductListDto(
    Guid Id,
    string Name,
    List<string> Category,
    string? Description,
    string? ImageFile,
    decimal Price
    );
