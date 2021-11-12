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
        public object Get(int enterpriseId);
        public ProductViewModel<AllergenDto, CategoryDto> GetById(int enterpriseId, int id);
        public Task<int> Create(int enterpriseId, ProductCoreDto<int, int> dto);
        public Task Update(int enterpriseId, ProductDto<int, int> dto, ICollection<int> removeAllergensIds, ICollection<int> removeCategoriesIds);
        public Task Delete(int enterpriseId, int id);
    }
}
