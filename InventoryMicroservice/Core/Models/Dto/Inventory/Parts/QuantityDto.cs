using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Models.Dto.Inventory.Parts
{
    public class QuantityDto
    {
        public ushort NumOfAvailable { get; set; }
        public ushort NumOfSettled { get; set; }
        public ushort NumOfSpoiled { get; set; }

        public QuantityDto()
        {

        }
    }
}
