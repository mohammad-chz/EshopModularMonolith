using Basket.Basket.Exceptions;
using Basket.Data;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool Issuccess);

public class DeleteBasketValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketValidator()
    {
        RuleFor(b => b.UserName)
            .NotEmpty()
            .WithMessage("نام کاربری سبد خرید وارد نشده.");
    }
}


internal class DeleteBasketHandler(BasketDbContext context) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await context.ShoppingCarts
            .SingleOrDefaultAsync(s => s.UserName == command.UserName.Trim(), cancellationToken)
                ?? throw new BasketNotFoundException(command.UserName.Trim());

        context.Remove(basket);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteBasketResult(true);
    }
}
