using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Entities;

namespace InventoryMicroservice.Core.Models.Dto.Category
{
    public class CategoryDto : CategoryCoreDto
    {
        public int Id { get; set; }

        public CategoryDto(CategoryToProduct values)
        {
            Id = values.CategoryId;
            Code = values.Category.Code;
            Name = values.Category.Name;
            Description = values.Category.Description;
        }
    }
}
