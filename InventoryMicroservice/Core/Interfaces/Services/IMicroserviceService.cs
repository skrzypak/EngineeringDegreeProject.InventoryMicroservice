using System;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public object GetAvaliableInventoryItems(int enterpriseId);
        public object GetInventorySummary(int enterpriseId, DateTime startDate, DateTime endDate);
        public void UpdateInventoryItemManual(int enterpriseId, int id, InventoryOperationType operationType, ushort quantity);
        public void UpdateInventoryProduct(int enterpriseId, int productId, InventoryOperationType operationType, ushort quantity, ushort unitMeasureValue);
    }
}
