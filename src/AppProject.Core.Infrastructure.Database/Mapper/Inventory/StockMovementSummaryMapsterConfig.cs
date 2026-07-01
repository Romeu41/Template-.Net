using System;
using AppProject.Core.Infrastructure.Database.Entities.Inventory;
using AppProject.Core.Models.Inventory;
using Mapster;

namespace AppProject.Core.Infrastructure.Database.Mapper.Inventory;

public class StockMovementSummaryMapsterConfig : IRegisterMapsterConfig
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TbStockMovement, StockMovementSummary>()
            .Map(dest => dest.ProductName, src => src.Product.Name);
    }
}
