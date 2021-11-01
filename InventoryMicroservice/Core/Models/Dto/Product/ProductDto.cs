using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Models.Dto.Product
{
    public class ProductDto<TA, TC> : ProductCoreDto<TA, TC>
    {
        public int Id { get; set; }
    }
}
