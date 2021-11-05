using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryMicroservice.Core.Models.Dto.Inventory
{
    public class InventoryCoreDto
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int DeliveryDocumentId { get; set; }
        public int DeliveryProductId { get; set; }
        [Range(0, 65535)]
        public ushort NumOfAvailable { get; set; }
        [Range(0, 65535)]
        public ushort NumOfSettled { get; set; }
        [Range(0, 65535)]
        public ushort NumOfSpoiled { get; set; }
        [Range(0, 65535)]
        public ushort Quantity { get; set; }
        [Range(0, 65535)]
        public int UnitMeasureValue { get; set; }
        public decimal UnitNetPrice { get; set; }
        public decimal PercentageVat { get; set; }
        public decimal GrossValue { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
