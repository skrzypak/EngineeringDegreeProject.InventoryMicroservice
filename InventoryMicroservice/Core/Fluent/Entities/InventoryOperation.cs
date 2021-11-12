using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Fluent.Entities
{
    public class InventoryOperation : IEntity
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public ushort Quantity { get; set; }
        public InventoryOperationType Operation { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public virtual Inventory Inventory { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }

    }
}
