using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Models.Dto.Product
{
    public abstract class ProductBasicDto
    {
        [MaxLength(6)]
        public virtual string Code { get; set; }
        [MinLength(3), MaxLength(300)]
        public virtual string Name { get; set; }
        [MaxLength(3000)]
        public virtual string Description { get; set; }
        public virtual UnitType Unit { get; set; }
    }
}
