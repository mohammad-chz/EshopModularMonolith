using Basket.Basket.Exceptions;
using Basket.Data;
using Basket.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketValidator()
    {
        RuleFor(b => b.UserName)
            .NotEmpty()
            .WithMessage("نام کاربری سبد خرید وارد نشده.");
    }
}


internal class DeleteBasketHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.UserName, asNoTracking: false, cancellationToken)
            ?? throw new BasketNotFoundException(command.UserName);

        return new DeleteBasketResult(await repository.DeleteBasket(basket, cancellationToken));
    }
}
