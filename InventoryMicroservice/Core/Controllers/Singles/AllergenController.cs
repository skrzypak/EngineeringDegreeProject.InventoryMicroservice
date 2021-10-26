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
    [Route("[controller]")]
    public class AllergenController : ControllerBase
    {
        private readonly ILogger<AllergenController> _logger;
        private readonly IAllergenService _allergenService;

        public AllergenController(ILogger<AllergenController> logger, IAllergenService allergenService)
        {
            _logger = logger;
            _allergenService = allergenService;
        }

        [HttpGet("{id}")]
        public ActionResult<AllergenViewModel<ProductDto>> Get([FromRoute] int id)
        {
            var categoryViewModel = _allergenService.Get(id);
            return Ok(categoryViewModel);
        }

        [HttpPost]
        public ActionResult Create([FromBody] AllergenBasicDto dto)
        {
            var categoryId = _allergenService.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = categoryId }, null);
        }

        [HttpPatch]
        public ActionResult Update([FromBody] AllergenDto dto)
        {
            _allergenService.Update(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _allergenService.Delete(id);
            return NoContent();
        }
    }
}
