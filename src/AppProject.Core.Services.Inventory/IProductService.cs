using System;
using AppProject.Core.Models.Inventory;
using AppProject.Models;

namespace AppProject.Core.Services.Inventory;

public interface IProductService
    : ITransientService,
    IGetEntity<GetByIdRequest<Guid>, EntityResponse<Product>>,
    IPostEntity<CreateOrUpdateRequest<Product>, KeyResponse<Guid>>,
    IPutEntity<CreateOrUpdateRequest<Product>, KeyResponse<Guid>>,
    IDeleteEntity<DeleteRequest<Guid>, EmptyResponse>
{
}
