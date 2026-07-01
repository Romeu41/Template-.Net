using AppProject.Core.Services.Inventory;
using AppProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppProject.Core.Controllers.Inventory
{
    [Route("api/inventory/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductSummaryController(IProductSummaryService productSummaryService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSummariesAsync([FromQuery] SearchRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await productSummaryService.GetSummariesAsync(request, cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> GetSummaryAsync([FromQuery] GetByIdRequest<Guid> request, CancellationToken cancellationToken)
        {
            return this.Ok(await productSummaryService.GetSummaryAsync(request, cancellationToken));
        }
    }
}
