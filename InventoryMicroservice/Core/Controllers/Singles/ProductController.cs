using System.Collections.Generic;
using System.Threading.Tasks;
using Authentication;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/inventory/1.0.0/products")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IHeaderContextService _headerContextService;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _productService = productService;
            _headerContextService = headerContextService;
        }

        [HttpGet]
        public ActionResult<object> Get([FromQuery] int espId)
        {
            var response = _productService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductViewModel<AllergenDto, CategoryDto>> GetById([FromQuery] int espId, [FromRoute] int id)
        {
            var productViewModel = _productService.GetById(espId, id);
            return Ok(productViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromQuery] int espId, [FromBody] ProductCoreDto<int, int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            var id = await _productService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(Get), new { id = id, espId = espId }, null);
        }

        [HttpPatch]
        public async Task<ActionResult> Update(
            [FromQuery] int espId,
            [FromBody] ProductDto<int, int> dto)
        {
            int eudId = _headerContextService.GetEudId();
            await _productService.Update(espId, eudId, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromQuery] int espId, [FromRoute] int id)
        {
            int eudId = _headerContextService.GetEudId();
            await _productService.Delete(espId, eudId, id);
            return NoContent();
        }
    }
}
