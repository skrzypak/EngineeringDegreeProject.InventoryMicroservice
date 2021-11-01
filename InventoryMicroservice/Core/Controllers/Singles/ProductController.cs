using System;
using System.Collections.Generic;
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
    [Route("[controller]")]
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
        public ActionResult<object> Get()
        {
            var response = _productService.Get();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductViewModel<AllergenDto, CategoryDto>> Get([FromRoute] int id)
        {
            var productViewModel = _productService.GetById(id);
            return Ok(productViewModel);
        }

        [HttpPost]
        public ActionResult Create([FromBody] ProductCoreDto<int, int> dto)
        {
            var id = _productService.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = id }, null);
        }

        [HttpPatch]
        public ActionResult Update(
            [FromBody] ProductDto<int, int> dto, 
            [FromQuery] ICollection<int> removeAllergensIds,
            [FromQuery] ICollection<int> removeCategoriesIds)
        {
            _productService.Update(dto, removeAllergensIds, removeCategoriesIds);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _productService.Delete(id);
            return NoContent();
        }
    }
}
