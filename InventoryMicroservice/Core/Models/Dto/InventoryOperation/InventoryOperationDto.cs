using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Models.Dto.Inventory
{
    public class InventoryOperationDto : InventoryOperationCoreDto
    {
        public int Id { get; set; }
    }
}
