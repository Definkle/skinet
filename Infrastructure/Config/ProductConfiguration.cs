using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entityTypeBuilder)
    {
        entityTypeBuilder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        entityTypeBuilder.Property(p => p.Name).IsRequired();
    }
}
