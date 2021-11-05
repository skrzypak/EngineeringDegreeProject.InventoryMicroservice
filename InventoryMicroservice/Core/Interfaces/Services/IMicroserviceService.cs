using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public object GetAvaliableInventoryItems();
        public object GetInventorySummary(DateTime startDate, DateTime endDate);
        public void UpdateInventoryItemManual(int id, InventoryOperationType operationType, ushort quantity);
        public void UpdateInventoryProduct(int productId, InventoryOperationType operationType, ushort quantity, ushort unitMeasureValue);
    }
}
