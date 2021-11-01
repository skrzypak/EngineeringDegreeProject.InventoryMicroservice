using System;
using System.ComponentModel.DataAnnotations;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Models.Dto.Inventory
{
    public class InventoryOperationCoreDto
    {
        [Range(0, 65535)]
        public int Quantity { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
        public InventoryOperationType Operation { get; set; }
    }
}
