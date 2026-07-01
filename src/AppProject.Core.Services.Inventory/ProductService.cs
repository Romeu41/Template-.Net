using System;
using AppProject.Core.Infrastructure.Database;
using AppProject.Core.Infrastructure.Database.Entities.Inventory;
using AppProject.Core.Models.Inventory;
using AppProject.Core.Services.Auth;
using AppProject.Exceptions;
using AppProject.Models;
using AppProject.Models.Auth;
using Mapster;

namespace AppProject.Core.Services.Inventory;

public class ProductService(
    IDatabaseRepository databaseRepository,
    IPermissionService permissionService)
    : BaseService, IProductService
{
    public async Task<EntityResponse<Product>> GetEntityAsync(GetByIdRequest<Guid> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);

        var product = await databaseRepository.GetFirstOrDefaultAsync<TbProduct, Product>(
            query => query.Where(x => x.Id == request.Id),
            cancellationToken);

        if (product == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        return new EntityResponse<Product>
        {
            Entity = product
        };
    }

    public async Task<KeyResponse<Guid>> PostEntityAsync(CreateOrUpdateRequest<Product> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);
        await this.ValidateProductAsync(request.Entity, cancellationToken);

        var tbProduct = request.Entity.Adapt<TbProduct>();
        await databaseRepository.InsertAndSaveAsync(tbProduct, cancellationToken);

        return new KeyResponse<Guid>
        {
            Id = tbProduct.Id
        };
    }

    public async Task<KeyResponse<Guid>> PutEntityAsync(CreateOrUpdateRequest<Product> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);
        await this.ValidateProductAsync(request.Entity, cancellationToken);

        var tbProduct = await databaseRepository.GetFirstOrDefaultAsync<TbProduct>(
            query => query.Where(x => x.Id == request.Entity.Id),
            cancellationToken);

        if (tbProduct == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        request.Entity.Adapt(tbProduct);

        await databaseRepository.UpdateAndSaveAsync(tbProduct, cancellationToken);

        return new KeyResponse<Guid>
        {
            Id = tbProduct.Id
        };
    }

    public async Task<EmptyResponse> DeleteEntityAsync(DeleteRequest<Guid> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);

        var tbProduct = await databaseRepository.GetFirstOrDefaultAsync<TbProduct>(
            query => query.Where(x => x.Id == request.Id),
            cancellationToken);

        if (tbProduct == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        if (await databaseRepository.HasAnyAsync<TbStockMovement>(
            query => query.Where(x => x.ProductId == tbProduct.Id),
            cancellationToken))
        {
            throw new AppException(ExceptionCode.Inventory_Product_ContainsStockMovements);
        }

        await databaseRepository.DeleteAndSaveAsync(tbProduct, cancellationToken);

        return new EmptyResponse();
    }

    private async Task ValidateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        if (await databaseRepository.HasAnyAsync<TbProduct>(
            query => query.Where(x => x.Name == product.Name && x.Id != product.Id),
            cancellationToken))
        {
            throw new AppException(ExceptionCode.Inventory_Product_DuplicateName);
        }

        if (!string.IsNullOrWhiteSpace(product.Code) && await databaseRepository.HasAnyAsync<TbProduct>(
            query => query.Where(x => x.Code == product.Code && x.Id != product.Id),
            cancellationToken))
        {
            throw new AppException(ExceptionCode.Inventory_Product_DuplicateCode);
        }
    }
}
