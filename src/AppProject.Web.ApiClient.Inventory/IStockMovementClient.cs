using System;
using AppProject.Models;
using AppProject.Web.Models.Inventory;
using Refit;

namespace AppProject.Web.ApiClient.Inventory;

public interface IStockMovementClient
{
    [Get("/api/inventory/StockMovement/Get")]
    public Task<EntityResponse<StockMovement>> GetAsync([Query] GetByIdRequest<Guid> request, CancellationToken cancellationToken = default);

    [Post("/api/inventory/StockMovement/Post")]
    public Task<KeyResponse<Guid>> PostAsync([Body] CreateOrUpdateRequest<StockMovement> request, CancellationToken cancellationToken = default);

    [Put("/api/inventory/StockMovement/Put")]
    public Task<KeyResponse<Guid>> PutAsync([Body] CreateOrUpdateRequest<StockMovement> request, CancellationToken cancellationToken = default);

    [Delete("/api/inventory/StockMovement/Delete")]
    public Task<EmptyResponse> DeleteAsync([Query] DeleteRequest<Guid> request, CancellationToken cancellationToken = default);
}
