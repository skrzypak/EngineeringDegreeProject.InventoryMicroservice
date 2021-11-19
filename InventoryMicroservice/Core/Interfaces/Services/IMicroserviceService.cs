using System;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public object GetAvaliableInventoryItems(int espId);
        public object GetInventorySummary(int espId, DateTime startDate, DateTime endDate);
        public void UpdateInventoryItemManual(int espId, int eudId, int id, InventoryOperationType operationType, ushort quantity);
        public void UpdateInventoryProduct(int espId, int eudId, int productId, InventoryOperationType operationType, ushort quantity, ushort unitMeasureValue);
    }
}
