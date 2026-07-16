using Basket.Basket.Models;
using Basket.Data;
using Basket.Data.Repository;

namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(CreateShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid Id);

public class CreateBasketValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketValidator()
    {
        RuleFor(b => b.ShoppingCart.UserName)
            .NotEmpty()
            .WithMessage("نام کاربری وارد نشده.");
    }
}

internal class CreateBasketHandler(IBasketRepository repository) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = CreateNewBasket(command.ShoppingCart);

        await repository.CreateBasket(basket, cancellationToken);

        return new CreateBasketResult(basket.Id);
    }

    private static ShoppingCart CreateNewBasket(CreateShoppingCartDto shoppingCart)
    {
        var newBasket = ShoppingCart.Create(shoppingCart.UserName);

        shoppingCart.Items.ForEach(item =>
        {
            newBasket.AddItem(
                item.ProductId,
                item.Quantity,
                item.Color,
                item.Price,
                item.ProductName
                );
        });

        return newBasket;
    }
}
