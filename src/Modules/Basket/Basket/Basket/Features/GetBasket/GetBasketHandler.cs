using Basket.Basket.Exceptions;
using Basket.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

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

internal class GetBasketHandler(BasketDbContext context) : IQueryhandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var userName = query.UserName.Trim();

        var basket = await context.ShoppingCarts
            .AsNoTracking()
            .Where(s => s.UserName == userName)
            .ProjectToType<ShoppingCartDto>()
            .SingleOrDefaultAsync(cancellationToken) ?? throw new BasketNotFoundException(userName);

        return new GetBasketResult(basket);
    }
}
