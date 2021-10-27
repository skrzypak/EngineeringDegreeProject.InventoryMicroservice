using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Fluent.Entities
{
    public class Inventory : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int InvoicingSupplierId { get; set; }
        public int InvoicingDeliveryDocumentId { get; set; }
        public int InvoicingDeliveryProductId { get; set; }
        public ushort NumOfAvailable { get; set; }
        public ushort NumOfSettled { get; set; }
        public ushort NumOfSpoiled { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<InventoryOperation> InventoryOperations { get; set; }
    }
}
