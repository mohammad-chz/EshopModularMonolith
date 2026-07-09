using Basket.Basket.Models;
using Basket.Data;

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

internal class CreateBasketHandler(BasketDbContext context) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = CreateNewBasket(command.ShoppingCart);

        context.ShoppingCarts.Add(shoppingCart);

        await context.SaveChangesAsync(cancellationToken);

        return new CreateBasketResult(shoppingCart.Id);
    }

    private static ShoppingCart CreateNewBasket(CreateShoppingCartDto shoppingCart)
    {
        var newBasket = ShoppingCart.Create(shoppingCart.UserName.Trim());

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
