using Catalog.Data;
using Catalog.Products.Exceptions;
using FluentValidation;
using Shared.CQRS;

namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage("شناسه محصول وارد نشده.");
    }
}

internal class DeleteProductHandler(CatalogDbContext context) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync([command.Id], cancellationToken) ?? throw new ProductNotFoundException(command.Id);

        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}
