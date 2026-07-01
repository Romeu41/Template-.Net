using System;
using AppProject.Core.Models.Inventory;
using AppProject.Models;

namespace AppProject.Core.Services.Inventory;

public interface IProductSummaryService
    : ITransientService,
    IGetSummaries<SearchRequest, SummariesResponse<ProductSummary>>,
    IGetSummary<GetByIdRequest<Guid>, SummaryResponse<ProductSummary>>
{
}
