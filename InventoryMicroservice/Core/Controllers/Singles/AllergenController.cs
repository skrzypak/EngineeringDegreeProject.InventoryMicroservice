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
    [Route("/api/inventory/1.0.0/allergens")]
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
        public ActionResult<object> Get()
        {
            var response = _allergenService.Get();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<AllergenViewModel<ProductBasicWithIdDto>> Get([FromRoute] int id)
        {
            var categoryViewModel = _allergenService.GetById(id);
            return Ok(categoryViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AllergenCoreDto dto)
        {
            var categoryId = await _allergenService.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = categoryId }, null);
        }

        [HttpPatch]
        public async Task<ActionResult> Update([FromBody] AllergenDto dto)
        {
            await _allergenService.Update(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _allergenService.Delete(id);
            return NoContent();
        }
    }
}
