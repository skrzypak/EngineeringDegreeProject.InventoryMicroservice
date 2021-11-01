using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryMicroservice.Core.Models.Dto.Category
{
    public class CategoryCoreDto
    {
        [MaxLength(6)]
        public string Code { get; set; }
        [MinLength(3), MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
    }
}
