using System;
using AppProject.Core.Infrastructure.Database.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppProject.Core.Infrastructure.Database.EntityTypeConfiguration.Inventory;

public class TbStockMovementConfiguration : IEntityTypeConfiguration<TbStockMovement>
{
    public void Configure(EntityTypeBuilder<TbStockMovement> builder)
    {
        builder.HasIndex(x => new { x.ProductId, x.MovementDate });
    }
}
