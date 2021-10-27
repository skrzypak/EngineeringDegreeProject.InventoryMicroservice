using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Models.Dto.Inventory
{
    public class InventoryOperationBasicDto
    {
        public int Quantity { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
        public InventoryOperationType Operation { get; set; }
    }
}
