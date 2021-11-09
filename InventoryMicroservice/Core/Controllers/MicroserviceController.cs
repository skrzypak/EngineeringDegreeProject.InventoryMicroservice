using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Enums;
using InventoryMicroservice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers
{
    [ApiController]
    [Route("/api/inventory/1.0.0/msv")]
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
        public ActionResult<object> GetAvaliableInventoryItems()
        {
            var inventoryItems = _microserviceService.GetAvaliableInventoryItems();
            return Ok(inventoryItems);
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateInventoryItemManual([FromRoute] int id, [FromQuery] InventoryOperationType operationType, [FromQuery] ushort quantity)
        {
            if(operationType == InventoryOperationType.Add || operationType == InventoryOperationType.Remove)
            {
                return BadRequest("Invalid type of operation");
            }

            _microserviceService.UpdateInventoryItemManual(id, operationType, quantity);

            return NoContent();
        }

        [HttpPatch("products/{productId}")]
        public ActionResult UpdateInventoryProduct([FromRoute] int productId, [FromQuery] InventoryOperationType operationType, [FromQuery] ushort quantity, [FromQuery]  ushort unitMeasureValue)
        {
            if (operationType == InventoryOperationType.Add || operationType == InventoryOperationType.Remove)
            {
                return BadRequest("Invalid type of operation");
            }

            _microserviceService.UpdateInventoryProduct(productId, operationType, quantity, unitMeasureValue);

            return NoContent();
        }

        [HttpGet("summary")]
        public ActionResult<object> GetInventorySummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = _microserviceService.GetInventorySummary(startDate, endDate);
            return Ok(response);
        }

    }
}
