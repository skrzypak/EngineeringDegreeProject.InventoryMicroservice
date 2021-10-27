using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Inventory.Parts;

namespace InventoryMicroservice.Core.Models.Dto.Inventory
{
    public class InventoryDto : InventoryBasicDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public InvoicingDto InvoicingDto { get; set;}
    }
}
