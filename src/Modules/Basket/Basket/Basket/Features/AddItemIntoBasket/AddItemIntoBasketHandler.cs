using Basket.Basket.Exceptions;
using Basket.Data;
using Basket.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.AddItemIntoBasket;


public record AddItemIntoBasketCommand(string UserName, AddShoppingCartItem ShoppingCartItem) : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketValidator()
    {
        RuleFor(b => b.UserName)
            .NotEmpty().WithMessage("نام کاربری وارد نشده.");

        RuleFor(b => b.ShoppingCartItem.ProductId)
            .NotEmpty().WithMessage("شناسه محصول وارد نشده.");

        RuleFor(b => b.ShoppingCartItem.ProductName)
            .NotEmpty().WithMessage("نام محصول وارد نشده.");

        RuleFor(b => b.ShoppingCartItem.Quantity)
            .GreaterThan(0).WithMessage("تعداد باید بیشتر از صفر باشد.");

        RuleFor(b => b.ShoppingCartItem.Price)
            .GreaterThan(0).WithMessage("قیمت باید بیشتر از صفر باشد.");
    }
}
internal class AddItemIntoBasketHandler(IBasketRepository repository) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.UserName, asNoTracking: false, cancellationToken)
            ?? throw new BasketNotFoundException(command.UserName);

        basket.AddItem(
           command.ShoppingCartItem.ProductId,
           command.ShoppingCartItem.Quantity,
           command.ShoppingCartItem.Color,
           command.ShoppingCartItem.Price,
           command.ShoppingCartItem.ProductName
           );

        await repository.SaveChangesAsync(cancellationToken);

        return new AddItemIntoBasketResult(basket.Id);
    }
}
