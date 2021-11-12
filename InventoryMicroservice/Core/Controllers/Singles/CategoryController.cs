using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/inventory/1.0.0/{enterpriseId}/categories")]
    public class CategoryController: ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }


        [HttpGet]
        public ActionResult<object> Get([FromRoute] int enterpriseId)
        {
            var response = _categoryService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryViewModel<ProductBasicWithIdDto>> GetById([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            var categoryViewModel = _categoryService.GetById(enterpriseId, id);
            return Ok(categoryViewModel);
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int enterpriseId, [FromBody] CategoryCoreDto dto)
        {
            var categoryId = _categoryService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(Get), new { enterpriseId = enterpriseId, id = categoryId }, null);
        }

        [HttpPatch]
        public ActionResult Update([FromRoute] int enterpriseId, [FromBody] CategoryDto dto)
        {
            _categoryService.Update(enterpriseId, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            _categoryService.Delete(enterpriseId, id);
            return NoContent();
        }
    }
}
