using Authentication;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/inventory/1.0.0/categories")]
    public class CategoryController: ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IHeaderContextService _headerContextService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _headerContextService = headerContextService;
        }


        [HttpGet]
        public ActionResult<object> Get([FromQuery] int espId)
        {
            var response = _categoryService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryViewModel<ProductBasicWithIdDto>> GetById([FromQuery] int espId, [FromRoute] int id)
        {
            var categoryViewModel = _categoryService.GetById(espId, id);
            return Ok(categoryViewModel);
        }

        [HttpPost]
        public ActionResult Create([FromQuery] int espId, [FromBody] CategoryCoreDto dto)
        {
            int eudId = _headerContextService.GetEudId();
            var categoryId = _categoryService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(Get), new { id = categoryId, espId = espId }, null);
        }

        [HttpPatch]
        public ActionResult Update([FromQuery] int espId, [FromBody] CategoryDto dto)
        {
            int eudId = _headerContextService.GetEudId();
            _categoryService.Update(espId, eudId, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromQuery] int espId, [FromRoute] int id)
        {
            int eudId = _headerContextService.GetEudId();
            _categoryService.Delete(espId, eudId, id);
            return NoContent();
        }
    }
}
