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
            Calories = values.Product.Calories;
            Proteins = values.Product.Proteins;
            Carbohydrates = values.Product.Carbohydrates;
            Fats = values.Product.Fats;
        }

        public ProductBasicWithIdDto(InventoryMicroservice.Core.Fluent.Entities.Product values)
        {
            Id = values.Id;
            Name = values.Name;
            Code = values.Code;
            Description = values.Description;
            Unit = values.Unit;
            Calories = values.Calories;
            Proteins = values.Proteins;
            Carbohydrates = values.Carbohydrates;
            Fats = values.Fats;
        }

    }
}
