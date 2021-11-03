using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Product;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IProductService
    {
        public object Get();
        public ProductViewModel<AllergenDto, CategoryDto> GetById(int id);
        public Task<int> Create(ProductCoreDto<int, int> dto);
        public Task Update(ProductDto<int, int> dto, ICollection<int> removeAllergensIds, ICollection<int> removeCategoriesIds);
        public Task Delete(int id);
    }
}
