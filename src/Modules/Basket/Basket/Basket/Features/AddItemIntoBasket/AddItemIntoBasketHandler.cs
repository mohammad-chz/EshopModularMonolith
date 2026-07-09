using Basket.Basket.Exceptions;
using Basket.Basket.Models;
using Basket.Data;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.AddItemIntoBasket;


public record AddItemIntoBasketCommand(AddShoppingCartItem ShoppingCartItem) : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketValidator()
    {
        RuleFor(b => b.ShoppingCartItem.UserName)
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
internal class AddItemIntoBasketHandler(BasketDbContext context) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        var userName = command.ShoppingCartItem.UserName.Trim();

        var basket = await context.ShoppingCarts
            .Include(s => s.Items)
            .SingleOrDefaultAsync(s => s.UserName == userName, cancellationToken)
            ?? throw new BasketNotFoundException(userName);

        basket.AddItem(
           command.ShoppingCartItem.ProductId,
           command.ShoppingCartItem.Quantity,
           command.ShoppingCartItem.Color,
           command.ShoppingCartItem.Price,
           command.ShoppingCartItem.ProductName
           );

        await context.SaveChangesAsync(cancellationToken);

        return new AddItemIntoBasketResult(basket.Id);
    }
}
