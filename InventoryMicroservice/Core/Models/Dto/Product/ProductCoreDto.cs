using System;
using System.Collections.Generic;
using Comunication.Shared;

namespace InventoryMicroservice.Core.Models.Dto.Product
{
    public class ProductCoreDto<TA, TC> : ProductBasicDto
    {
        public virtual ICollection<TA> Allergens { get; set; }
        public virtual TC Category { get; set; }
    }
}
