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

        public CategoryDto()
        {
        }

        public CategoryDto(InventoryMicroservice.Core.Fluent.Entities.Category values)
        {
            Id = values.Id;
            Code = values.Code;
            Name = values.Name;
            Description = values.Description;
        }
    }
}
