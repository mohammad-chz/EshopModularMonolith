using Basket.Basket.Exceptions;
using Basket.Data;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId) : ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(bool IsSuucess);

public class RemoveItemFromBasketValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketValidator()
    {
        RuleFor(b => b.UserName)
            .NotEmpty().WithMessage("نام کاربری وارد نشده.");

        RuleFor(b => b.ProductId)
            .NotEmpty().WithMessage("شناسه محصول وارد نشده.");
    }
}

internal class RemoveItemFromBasketHandler(BasketDbContext context) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var userName = command.UserName.Trim();

        var basket = await context.ShoppingCarts
            .Include(s => s.Items)
            .SingleOrDefaultAsync(s => s.UserName == userName, cancellationToken)
            ?? throw new BasketNotFoundException(userName);

        basket.RemoveItem(command.ProductId);

        await context.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(true);
    }
}
