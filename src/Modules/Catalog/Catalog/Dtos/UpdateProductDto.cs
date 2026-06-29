namespace Catalog.Dtos;

public record UpdateProductDto(
    Guid Id,
    string Name,
    List<string> Category,
    string? Description,
    string? ImageFile,
    decimal Price);
