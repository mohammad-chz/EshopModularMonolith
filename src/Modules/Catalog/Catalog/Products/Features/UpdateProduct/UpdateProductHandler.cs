using Catalog.Data;
using Catalog.Data.Constants;
using Catalog.Products.Exceptions;
using FluentValidation;
using Shared.CQRS;

namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(UpdateProductDto Product) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Product.Id)
            .NotEmpty()
            .WithMessage("شناسه محصول وارد نشده.");

        RuleFor(p => p.Product.Name)
            .NotEmpty()
            .WithMessage("نام محصول وارد نشده")
            .MaximumLength(ProductConstants.NameMaxLength)
            .WithMessage($"تعداد کاراکترها نباید بیشتر از {ProductConstants.NameMaxLength} باشد.");

        RuleForEach(x => x.Product.Category).NotEmpty().WithMessage("نام دسته‌بندی معتبر نیست.");

        RuleFor(p => p.Product.Category).NotEmpty().WithMessage("دسته بندی انتخاب نشده.");

        RuleFor(p => p.Product.Price)
            .NotEmpty().WithMessage("قیمت وارد نشده")
            .GreaterThanOrEqualTo(0).WithMessage("قیمت نباید کمتر از 0 باشد.");

        RuleFor(p => p.Product.Name)
            .MaximumLength(ProductConstants.DescriptionMaxLength)
            .WithMessage($"تعداد کاراکترها نباید بیشتر از {ProductConstants.DescriptionMaxLength} باشد.");
    }
}

internal class UpdateProductHandler(CatalogDbContext context) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync([command.Product.Id], cancellationToken) ?? throw new ProductNotFoundException(command.Product.Id);

        UpdateProduct(product, command.Product);

        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private static void UpdateProduct(Product product, UpdateProductDto dto)
    {
        product.Update(
           dto.Name,
           dto.Category,
           dto.Description,
           dto.ImageFile,
           dto.Price
           );
    }
}
