using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;

namespace Comunication.Shared.PayloadValue
{
    public class InventoryPayloadValue : IMessage
    {
        public int SupplierId { get; private set; }
        public int DocumentId { get; private set; }
        public ICollection<ItemsPayloadValue> Items { get; private set; } = new HashSet<ItemsPayloadValue>();
        public class ItemsPayloadValue
        {
            public CRUD Crud { get; set; }
            public int ProductId { get; set; }
            public int DocumentToProductId { get; set; }
            public ushort Quantity { get; set; } = 0;
            public int UnitMeasureValue { get; set; }
            public decimal UnitNetPrice { get; set; }
            public decimal PercentageVat { get; set; }
            public decimal GrossValue { get; set; }
            public DateTime? ExpirationDate { get; set; }
        }
        public int EspId { get; set; }
        public int EudId { get; set; }
    }
    
}
