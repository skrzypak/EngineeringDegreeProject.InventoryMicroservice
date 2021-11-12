using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Allergen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/inventory/1.0.0/{enterpriseId}/allergens")]
    public class AllergenController : ControllerBase
    {
        private readonly ILogger<AllergenController> _logger;
        private readonly IAllergenService _allergenService;

        public AllergenController(ILogger<AllergenController> logger, IAllergenService allergenService)
        {
            _logger = logger;
            _allergenService = allergenService;
        }

        [HttpGet]
        public ActionResult<object> Get([FromRoute] int enterpriseId)
        {
            var response = _allergenService.Get(enterpriseId);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<AllergenViewModel<ProductBasicWithIdDto>> GetById([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            var categoryViewModel = _allergenService.GetById(enterpriseId, id);
            return Ok(categoryViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] int enterpriseId, [FromBody] AllergenCoreDto dto)
        {
            var categoryId = await _allergenService.Create(enterpriseId, dto);
            return CreatedAtAction(nameof(Get), new { enterpriseId = enterpriseId, id = categoryId }, null);
        }

        [HttpPatch]
        public async Task<ActionResult> Update([FromRoute] int enterpriseId, [FromBody] AllergenDto dto)
        {
            await _allergenService.Update(enterpriseId, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int enterpriseId, [FromRoute] int id)
        {
            await _allergenService.Delete(enterpriseId, id);
            return NoContent();
        }
    }
}
