using Basket.Basket.Exceptions;
using Basket.Data;
using Basket.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId) : ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(bool IsSuccess);

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

internal class RemoveItemFromBasketHandler(IBasketRepository repository) : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.UserName, asNoTracking: false, cancellationToken)
            ?? throw new BasketNotFoundException(command.UserName);

        basket.RemoveItem(command.ProductId);

        await repository.SaveChangesAsync(cancellationToken);

        return new RemoveItemFromBasketResult(true);
    }
}
