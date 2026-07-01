using AppProject.Core.Models.Inventory;
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
    public class StockMovementSummaryController(IStockMovementSummaryService stockMovementSummaryService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSummariesAsync([FromQuery] StockMovementSummarySearchRequest request, CancellationToken cancellationToken)
        {
            return this.Ok(await stockMovementSummaryService.GetSummariesAsync(request, cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> GetSummaryAsync([FromQuery] GetByIdRequest<Guid> request, CancellationToken cancellationToken)
        {
            return this.Ok(await stockMovementSummaryService.GetSummaryAsync(request, cancellationToken));
        }
    }
}
