using System;
using AppProject.Models;
using AppProject.Web.Models.Inventory;
using Refit;

namespace AppProject.Web.ApiClient.Inventory;

public interface IProductSummaryClient
{
    [Get("/api/inventory/ProductSummary/GetSummaries")]
    public Task<SummariesResponse<ProductSummary>> GetSummariesAsync([Query] SearchRequest request, CancellationToken cancellationToken = default);

    [Get("/api/inventory/ProductSummary/GetSummary")]
    public Task<SummaryResponse<ProductSummary>> GetSummaryAsync([Query] GetByIdRequest<Guid> request, CancellationToken cancellationToken = default);
}
