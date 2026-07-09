using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations
{
    internal class ShoppingCartConfigurations : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(sh => sh.Id);

            builder.Property(sh => sh.Id)
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder.Property(sh => sh.UserName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(sh => sh.UserName)
                .IsUnique();

            builder.HasMany(sh => sh.Items)
                .WithOne()
                .HasForeignKey(shi => shi.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
