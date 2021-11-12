using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [Route("/api/inventory/1.0.0/{enterpriseId}/products")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<object> Get([FromRoute] int enterpriseId)
        {
            var response = _productService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductViewModel<AllergenDto, CategoryDto>> GetById([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            var productViewModel = _productService.GetById(enterpriseId, id);
            return Ok(productViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] int enterpriseId, [FromBody] ProductCoreDto<int, int> dto)
        {
            var id = await _productService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(Get), new { enterpriseId = enterpriseId, id = id }, null);
        }

        [HttpPatch]
        public async Task<ActionResult> Update(
            [FromRoute] int enterpriseId,
            [FromBody] ProductDto<int, int> dto, 
            [FromQuery] ICollection<int> removeAllergensIds,
            [FromQuery] ICollection<int> removeCategoriesIds)
        {
            await _productService.Update(enterpriseId, dto, removeAllergensIds, removeCategoriesIds);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            await _productService.Delete(enterpriseId, id);
            return NoContent();
        }
    }
}
