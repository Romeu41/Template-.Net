using System;
using AppProject.Core.Infrastructure.Database;
using AppProject.Core.Infrastructure.Database.Entities.Inventory;
using AppProject.Core.Models.Inventory;
using AppProject.Exceptions;
using AppProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Core.Services.Inventory;

public class StockMovementSummaryService(
    IDatabaseRepository databaseRepository)
    : BaseService, IStockMovementSummaryService
{
    public async Task<SummariesResponse<StockMovementSummary>> GetSummariesAsync(StockMovementSummarySearchRequest request, CancellationToken cancellationToken = default)
    {
        var searchText = request.SearchText?.Trim();

        var movementSummaries = await databaseRepository.GetByConditionAsync<TbStockMovement, StockMovementSummary>(
            query =>
            {
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(x =>
                        x.Product.Name.Contains(searchText) || (x.Product.Code ?? string.Empty).Contains(searchText));
                }

                if (request.ProductId.HasValue)
                {
                    query = query.Where(x => x.ProductId == request.ProductId.Value);
                }

                query = query.OrderByDescending(x => x.MovementDate).ThenBy(x => x.Id);

                if (request.Take.HasValue)
                {
                    query = query.Take(request.Take.Value);
                }

                return query;
            },
            cancellationToken);

        return new SummariesResponse<StockMovementSummary>
        {
            Summaries = movementSummaries
        };
    }

    public async Task<SummaryResponse<StockMovementSummary>> GetSummaryAsync(GetByIdRequest<Guid> request, CancellationToken cancellationToken = default)
    {
        var movementSummary = await databaseRepository.GetFirstOrDefaultAsync<TbStockMovement, StockMovementSummary>(
            query => query.Include(x => x.Product).Where(x => x.Id == request.Id),
            cancellationToken);

        if (movementSummary == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        return new SummaryResponse<StockMovementSummary>
        {
            Summary = movementSummary
        };
    }
}
