using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Models.Dto.Inventory.Parts
{
    public class InvoicingDto
    {
        public int InvoicingSupplierId { get; set; }
        public int InvoicingDeliveryDocumentId { get; set; }
        public int InvoicingDeliveryProductId { get; set; }

        public InvoicingDto()
        {

        }
    }
}
