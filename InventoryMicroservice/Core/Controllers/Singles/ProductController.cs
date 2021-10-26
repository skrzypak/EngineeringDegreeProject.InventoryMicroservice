using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("{id}")]
        public ActionResult<ProductViewModel> Get([FromRoute] int id)
        {
            var productViewModel = _productService.Get(id);
            return Ok(productViewModel);
        }

        [HttpPost]
        public ActionResult Create([FromBody] ProductCreateDto dto)
        {
            var productId = _productService.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = productId }, null);
        }

        [HttpPatch]
        public ActionResult Update([FromBody] ProductUpdateDto dto)
        {
            _productService.Update(dto);
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
