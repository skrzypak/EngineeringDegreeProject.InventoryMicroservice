using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Models.Dto.Product
{
    public class ProductCoreDto<TA, TC> : ProductBasicDto
    {
        public ICollection<TA> Allergens { get; set; }
        public ICollection<TC> Categories { get; set; }
    }
}
