using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryMicroservice.Core.Models.Dto.Inventory
{
    public class InventoryCoreDto
    {
        public int ProductId { get; set; }
        public int InvoicingSupplierId { get; set; }
        public int InvoicingDeliveryDocumentId { get; set; }
        public int InvoicingDeliveryProductId { get; set; }
        [Range(0, 65535)]
        public ushort NumOfAvailable { get; set; }
        [Range(0, 65535)]
        public ushort NumOfSettled { get; set; }
        [Range(0, 65535)]
        public ushort NumOfSpoiled { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
