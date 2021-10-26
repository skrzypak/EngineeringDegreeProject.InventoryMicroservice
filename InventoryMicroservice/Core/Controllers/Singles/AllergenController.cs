using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("[controller]")]
    public class AllergenController: ControllerBase
    {
        private readonly ILogger<AllergenController> _logger;

        public AllergenController(ILogger<AllergenController> logger)
        {
            _logger = logger;
        }
    }
}
