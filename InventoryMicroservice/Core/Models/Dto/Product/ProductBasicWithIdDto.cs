using System;
using InventoryMicroservice.Core.Fluent.Entities;

namespace InventoryMicroservice.Core.Models.Dto.Product
{
    public class ProductBasicWithIdDto : ProductBasicDto
    {
        public int Id { get; set; }

        public ProductBasicWithIdDto(AllergenToProduct values)
        {
            Id = values.Product.Id;
            Name = values.Product.Name;
            Code = values.Product.Code;
            Description = values.Product.Description;
            Unit = values.Product.Unit;
        }

        public ProductBasicWithIdDto(CategoryToProduct values)
        {
            Id = values.Product.Id;
            Name = values.Product.Name;
            Code = values.Product.Code;
            Description = values.Product.Description;
            Unit = values.Product.Unit;
        }

    }
}
