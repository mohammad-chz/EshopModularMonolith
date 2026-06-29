namespace Catalog.Dtos;

public record CreateProductDto(
    string Name,
    List<string> Category,
    string? Description,
    string? ImageFile,
    decimal Price);
