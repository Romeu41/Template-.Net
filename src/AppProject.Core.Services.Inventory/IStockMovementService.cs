using System;
using AppProject.Core.Models.Inventory;
using AppProject.Models;

namespace AppProject.Core.Services.Inventory;

public interface IStockMovementService
    : ITransientService,
    IGetEntity<GetByIdRequest<Guid>, EntityResponse<StockMovement>>,
    IPostEntity<CreateOrUpdateRequest<StockMovement>, KeyResponse<Guid>>,
    IPutEntity<CreateOrUpdateRequest<StockMovement>, KeyResponse<Guid>>,
    IDeleteEntity<DeleteRequest<Guid>, EmptyResponse>
{
}
