using Basket.Basket.Exceptions;
using Basket.Data.Repository;

namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

public class GetBasketValidator : AbstractValidator<GetBasketQuery>
{
    public GetBasketValidator()
    {
        RuleFor(b => b.UserName)
            .NotEmpty()
            .WithMessage("نام کاربری وارد نشده.");
    }
}

internal class GetBasketHandler(IBasketRepository repository) : IQueryhandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var userName = query.UserName.Trim();

        var basket = await repository.GetBasket(userName, ct: cancellationToken)
                    ?? throw new BasketNotFoundException(userName);

        var result = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(result);
    }
}
