using System;
using AppProject.Models;
using AppProject.Web.Models.Inventory;
using Refit;

namespace AppProject.Web.ApiClient.Inventory;

public interface IStockMovementSummaryClient
{
    [Get("/api/inventory/StockMovementSummary/GetSummaries")]
    public Task<SummariesResponse<StockMovementSummary>> GetSummariesAsync([Query] StockMovementSummarySearchRequest request, CancellationToken cancellationToken = default);

    [Get("/api/inventory/StockMovementSummary/GetSummary")]
    public Task<SummaryResponse<StockMovementSummary>> GetSummaryAsync([Query] GetByIdRequest<Guid> request, CancellationToken cancellationToken = default);
}
