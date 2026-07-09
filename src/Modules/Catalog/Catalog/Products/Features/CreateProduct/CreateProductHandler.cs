using Catalog.Data;
using Catalog.Data.Constants;
using FluentValidation;
using Shared.CQRS;

namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(CreateProductDto Product) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
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

internal class CreateProductHandler(CatalogDbContext context) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = CreateProduct(command.Product);

        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private static Product CreateProduct(CreateProductDto command)
    {
        return Product.Create(
            command.Name,
            command.Category,
            command.Description,
            command.ImageFile,
            command.Price);
    }
}
