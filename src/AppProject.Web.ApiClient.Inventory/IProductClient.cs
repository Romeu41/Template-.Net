using System;
using AppProject.Models;
using AppProject.Web.Models.Inventory;
using Refit;

namespace AppProject.Web.ApiClient.Inventory;

public interface IProductClient
{
    [Get("/api/inventory/Product/Get")]
    public Task<EntityResponse<Product>> GetAsync([Query] GetByIdRequest<Guid> request, CancellationToken cancellationToken = default);

    [Post("/api/inventory/Product/Post")]
    public Task<KeyResponse<Guid>> PostAsync([Body] CreateOrUpdateRequest<Product> request, CancellationToken cancellationToken = default);

    [Put("/api/inventory/Product/Put")]
    public Task<KeyResponse<Guid>> PutAsync([Body] CreateOrUpdateRequest<Product> request, CancellationToken cancellationToken = default);

    [Delete("/api/inventory/Product/Delete")]
    public Task<EmptyResponse> DeleteAsync([Query] DeleteRequest<Guid> request, CancellationToken cancellationToken = default);
}
