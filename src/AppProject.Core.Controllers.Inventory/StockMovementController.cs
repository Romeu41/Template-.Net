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
    public class StockMovementController(IStockMovementService stockMovementService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetByIdRequest<Guid> request, CancellationToken cancellationToken)
        {
            return this.Ok(await stockMovementService.GetEntityAsync(request, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateOrUpdateRequest<StockMovement> request, CancellationToken cancellationToken)
        {
            return this.Ok(await stockMovementService.PostEntityAsync(request, cancellationToken));
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] CreateOrUpdateRequest<StockMovement> request, CancellationToken cancellationToken)
        {
            return this.Ok(await stockMovementService.PutEntityAsync(request, cancellationToken));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] DeleteRequest<Guid> request, CancellationToken cancellationToken)
        {
            return this.Ok(await stockMovementService.DeleteEntityAsync(request, cancellationToken));
        }
    }
}
