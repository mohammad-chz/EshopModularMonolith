using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                 .HasColumnType("uniqueidentifier")
                 .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.Category)
               .IsRequired();

            builder.Property(p => p.ImageFile)
               .HasMaxLength(2048);

            builder.Property(p => p.Price)
              .IsRequired()
              .HasColumnType("decimal(18,0)");
        }
    }
}
