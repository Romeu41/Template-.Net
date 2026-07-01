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
    public class ProductController(IProductService productService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetByIdRequest<Guid> request, CancellationToken cancellationToken)
        {
            return this.Ok(await productService.GetEntityAsync(request, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateOrUpdateRequest<Product> request, CancellationToken cancellationToken)
        {
            return this.Ok(await productService.PostEntityAsync(request, cancellationToken));
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] CreateOrUpdateRequest<Product> request, CancellationToken cancellationToken)
        {
            return this.Ok(await productService.PutEntityAsync(request, cancellationToken));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] DeleteRequest<Guid> request, CancellationToken cancellationToken)
        {
            return this.Ok(await productService.DeleteEntityAsync(request, cancellationToken));
        }
    }
}
