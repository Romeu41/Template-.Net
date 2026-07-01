using System;
using AppProject.Core.Models.Inventory;
using AppProject.Models;

namespace AppProject.Core.Services.Inventory;

public interface IStockMovementSummaryService
    : ITransientService,
    IGetSummaries<StockMovementSummarySearchRequest, SummariesResponse<StockMovementSummary>>,
    IGetSummary<GetByIdRequest<Guid>, SummaryResponse<StockMovementSummary>>
{
}
