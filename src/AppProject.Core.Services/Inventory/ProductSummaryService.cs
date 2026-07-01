using System;
using AppProject.Core.Infrastructure.Database;
using AppProject.Core.Infrastructure.Database.Entities.Inventory;
using AppProject.Core.Models.Inventory;
using AppProject.Exceptions;
using AppProject.Models;

namespace AppProject.Core.Services.Inventory;

public class ProductSummaryService(
    IDatabaseRepository databaseRepository)
    : BaseService, IProductSummaryService
{
    public async Task<SummariesResponse<ProductSummary>> GetSummariesAsync(SearchRequest request, CancellationToken cancellationToken = default)
    {
        var searchText = request.SearchText?.Trim();

        var productSummaries = await databaseRepository.GetByConditionAsync<TbProduct, ProductSummary>(
            query =>
            {
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(x =>
                        x.Id.ToString().Contains(searchText) || x.Name.Contains(searchText) || (x.Code ?? string.Empty).Contains(searchText));
                }

                query = query.OrderBy(x => x.Name);

                if (request.Take.HasValue)
                {
                    query = query.Take(request.Take.Value);
                }

                return query;
            },
            cancellationToken);

        return new SummariesResponse<ProductSummary>
        {
            Summaries = productSummaries
        };
    }

    public async Task<SummaryResponse<ProductSummary>> GetSummaryAsync(GetByIdRequest<Guid> request, CancellationToken cancellationToken = default)
    {
        var productSummary = await databaseRepository.GetFirstOrDefaultAsync<TbProduct, ProductSummary>(
            query => query.Where(x => x.Id == request.Id),
            cancellationToken);

        if (productSummary == null)
        {
            throw new AppException(ExceptionCode.EntityNotFound);
        }

        return new SummaryResponse<ProductSummary>
        {
            Summary = productSummary
        };
    }
}
