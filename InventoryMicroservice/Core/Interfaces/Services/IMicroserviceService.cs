using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public object GetAvaliableInventoryProducts();
        // public object GetInventoryOperationsHistory();
        // public object GetInventoryProductOperationsHistory();
        // public void TransferItemToInventory(InventoryDto inventoryDto);
        public void UpdateQuantityInventoryProduct(int productId, ushort toSettling, ushort toSpoilling);
        //public void RemoveInventoryItem();
        //public void RemoveInventoryItemBySupplier();
        //public void RemoveInventoryItemByDeliveryDocument();
    }
}
