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
        public int SupplierId { get; set; }
        public int DocumentId { get; set; }
        public int DocumentToProductId { get; set; }
        public ushort NumOfAvailable { get; set; }
        public ushort NumOfSettled { get; set; } = 0;
        public ushort NumOfSpoiled { get; set; } = 0;
        public ushort NumOfDamaged { get; set; } = 0;
        public ushort Quantity { get; set; }
        public int UnitMeasureValue { get; set; }
        public decimal UnitNetPrice { get; set; }
        public decimal PercentageVat { get; set; }
        public decimal GrossValue { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<InventoryOperation> InventoryOperations { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
    }
}
