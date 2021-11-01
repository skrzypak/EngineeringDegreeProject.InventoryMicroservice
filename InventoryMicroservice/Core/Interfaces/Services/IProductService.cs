using System;
using System.Collections.Generic;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Product;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IProductService
    {
        public ProductViewModel<AllergenDto, CategoryDto> Get(int id);
        public int Create(ProductDto<int, int> dto);
        public void Update(ProductDto<int, int> dto, ICollection<int> removeAllergensIds, ICollection<int> removeCategoriesIds);
        public void Delete(int id);
    }
}
