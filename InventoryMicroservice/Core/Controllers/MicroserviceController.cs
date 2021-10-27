using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMicroserviceService _microserviceService;

        public MicroserviceController(
            ILogger<MicroserviceController> logger, 
            MicroserviceContext context, 
            IMicroserviceService microserviceService)
        {
            _logger = logger;
            _context = context;
            _microserviceService = microserviceService;
        }

        [HttpGet]
        public ActionResult<object> GetAvaliableInventoryProducts()
        {
            var inventoryItems = _microserviceService.GetAvaliableInventoryProducts();
            return Ok(inventoryItems);
        }

        [HttpPatch("{productId}")]
        public ActionResult UpdateQuantityInventoryProduct([FromRoute] int productId, [FromQuery] ushort toSettling, [FromQuery] ushort toSpoilling)
        {
            _microserviceService.UpdateQuantityInventoryProduct(productId, toSettling, toSpoilling);
            return NoContent();
        }


    }
}
