using System;
using Authentication;
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
        private readonly IMicroserviceService _microserviceService;
        private readonly IHeaderContextService _headerContextService;

        public MicroserviceController(
            ILogger<MicroserviceController> logger,  
            IMicroserviceService microserviceService,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _microserviceService = microserviceService;
            _headerContextService = headerContextService;
        }

        [HttpGet]
        public ActionResult<object> GetAvaliableInventoryItems([FromQuery] int espId)
        {
            var inventoryItems = _microserviceService.GetAvaliableInventoryItems(espId);
            return Ok(inventoryItems);
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateInventoryItemManual([FromQuery] int espId, [FromRoute] int id, [FromQuery] InventoryOperationType operationType, [FromQuery] ushort quantity)
        {
            if(operationType == InventoryOperationType.Add || operationType == InventoryOperationType.Remove)
            {
                return BadRequest("Invalid type of operation");
            }

            int eudId = _headerContextService.GetEudId();
            _microserviceService.UpdateInventoryItemManual(espId, eudId, id, operationType, quantity);

            return NoContent();
        }

        [HttpPatch("products/{productId}")]
        public ActionResult UpdateInventoryProduct([FromQuery] int espId, [FromRoute] int productId, [FromQuery] InventoryOperationType operationType, [FromQuery] ushort quantity, [FromQuery]  ushort unitMeasureValue)
        {
            if (operationType == InventoryOperationType.Add || operationType == InventoryOperationType.Remove)
            {
                return BadRequest("Invalid type of operation");
            }

            int eudId = _headerContextService.GetEudId();
            _microserviceService.UpdateInventoryProduct(espId, eudId, productId, operationType, quantity, unitMeasureValue);

            return NoContent();
        }

        [HttpGet("summary")]
        public ActionResult<object> GetInventorySummary([FromQuery] int espId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = _microserviceService.GetInventorySummary(espId, startDate, endDate);
            return Ok(response);
        }

    }
}
