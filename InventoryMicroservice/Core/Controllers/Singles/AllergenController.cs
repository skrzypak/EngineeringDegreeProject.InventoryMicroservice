using System.Threading.Tasks;
using Authentication;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Allergen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/inventory/1.0.0/allergens")]
    public class AllergenController : ControllerBase
    {
        private readonly ILogger<AllergenController> _logger;
        private readonly IAllergenService _allergenService;
        private readonly IHeaderContextService _headerContextService;

        public AllergenController(ILogger<AllergenController> logger, IAllergenService allergenService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _allergenService = allergenService;
            _headerContextService = headerContextService;
        }

        [HttpGet]
        public ActionResult<object> Get([FromQuery] int espId)
        {
            var response = _allergenService.Get(espId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<AllergenViewModel<ProductBasicWithIdDto>> GetById([FromQuery] int espId, [FromRoute] int id)
        {
            var categoryViewModel = _allergenService.GetById(espId, id);
            return Ok(categoryViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromQuery] int espId, [FromBody] AllergenCoreDto dto)
        {
            int eudId = _headerContextService.GetEudId();
            var categoryId = await _allergenService.Create(espId, eudId, dto);
            return CreatedAtAction(nameof(Get), new { id = categoryId, espId = espId }, null);
        }

        [HttpPatch]
        public async Task<ActionResult> Update([FromQuery] int espId, [FromBody] AllergenDto dto)
        {
            int eudId = _headerContextService.GetEudId();
            await _allergenService.Update(espId, eudId, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromQuery] int espId, [FromRoute] int id)
        {
            int eudId = _headerContextService.GetEudId();
            await _allergenService.Delete(espId, eudId, id);
            return NoContent();
        }
    }
}
