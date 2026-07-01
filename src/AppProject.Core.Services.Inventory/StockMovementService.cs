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

public class StockMovementService(
    IDatabaseRepository databaseRepository,
    IPermissionService permissionService)
    : BaseService, IStockMovementService
{
    public async Task<EntityResponse<StockMovement>> GetEntityAsync(GetByIdRequest<Guid> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);

        var movement = await databaseRepository.GetFirstOrDefaultAsync<TbStockMovement, StockMovement>(
            query => query.Where(x => x.Id == request.Id),
            cancellationToken);

        if (movement == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        return new EntityResponse<StockMovement>
        {
            Entity = movement
        };
    }

    public async Task<KeyResponse<Guid>> PostEntityAsync(CreateOrUpdateRequest<StockMovement> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);
        await this.ValidateMovementAsync(request.Entity, cancellationToken);

        var tbMovement = request.Entity.Adapt<TbStockMovement>();
        await databaseRepository.InsertAndSaveAsync(tbMovement, cancellationToken);

        return new KeyResponse<Guid>
        {
            Id = tbMovement.Id
        };
    }

    public async Task<KeyResponse<Guid>> PutEntityAsync(CreateOrUpdateRequest<StockMovement> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);
        await this.ValidateMovementAsync(request.Entity, cancellationToken);

        var tbMovement = await databaseRepository.GetFirstOrDefaultAsync<TbStockMovement>(
            query => query.Where(x => x.Id == request.Entity.Id),
            cancellationToken);

        if (tbMovement == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        request.Entity.Adapt(tbMovement);

        await databaseRepository.UpdateAndSaveAsync(tbMovement, cancellationToken);

        return new KeyResponse<Guid>
        {
            Id = tbMovement.Id
        };
    }

    public async Task<EmptyResponse> DeleteEntityAsync(DeleteRequest<Guid> request, CancellationToken cancellationToken = default)
    {
        await permissionService.ValidateCurrentUserPermissionAsync(PermissionType.System_ManageSettings, cancellationToken: cancellationToken);

        var tbMovement = await databaseRepository.GetFirstOrDefaultAsync<TbStockMovement>(
            query => query.Where(x => x.Id == request.Id),
            cancellationToken);

        if (tbMovement == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        await databaseRepository.DeleteAndSaveAsync(tbMovement, cancellationToken);

        return new EmptyResponse();
    }

    private async Task ValidateMovementAsync(StockMovement movement, CancellationToken cancellationToken)
    {
        if (movement.Quantity <= 0)
        {
            throw new AppException(ExceptionCode.Inventory_StockMovement_InvalidQuantity);
        }

        var product = await databaseRepository.GetFirstOrDefaultAsync<TbProduct>(
            query => query.Where(x => x.Id == movement.ProductId),
            cancellationToken);

        if (product == null)
        {
            throw new AppException(ExceptionCode.Inventory_StockMovement_ProductNotFound);
        }

        var existingBalance = await this.CalculateBalanceAsync(movement.ProductId, movement.Id, cancellationToken);
        var projectedBalance = movement.IsOutbound ? existingBalance - movement.Quantity : existingBalance + movement.Quantity;

        if (projectedBalance < 0)
        {
            throw new AppException(ExceptionCode.Inventory_StockMovement_InsufficientStock);
        }
    }

    private async Task<decimal> CalculateBalanceAsync(Guid productId, Guid? excludeMovementId, CancellationToken cancellationToken)
    {
        var movements = await databaseRepository.GetByConditionAsync<TbStockMovement>(
            query => query.Where(x => x.ProductId == productId && (!excludeMovementId.HasValue || x.Id != excludeMovementId.Value)),
            cancellationToken);

        decimal total = 0;

        foreach (var movement in movements)
        {
            total += movement.IsOutbound ? -movement.Quantity : movement.Quantity;
        }

        return total;
    }
}
