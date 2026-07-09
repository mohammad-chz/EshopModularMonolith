using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations
{
    internal class ShoppingCartItemConfigurations : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.HasKey(sh => sh.Id);

            builder.Property(sh => sh.Id)
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(sh => sh.ShoppingCartId)
                .IsRequired();

            builder.Property(sh => sh.ProductId)
                .IsRequired();

            builder.Property(sh => sh.Quantity)
                .IsRequired();

            builder.Property(sh => sh.Price)
                .IsRequired();

            builder.Property(sh => sh.ProductName)
                .IsRequired();
        }
    }
}
