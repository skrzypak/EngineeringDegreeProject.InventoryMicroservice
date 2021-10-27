using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Inventory.Parts;

namespace InventoryMicroservice.Core.Models.Dto.Inventory
{
    public abstract class InventoryBasicDto
    {
        public QuantityDto QuantityDto { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
