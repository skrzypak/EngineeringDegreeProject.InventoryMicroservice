using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Category;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        public object Get(int enterpriseId);
        public CategoryViewModel<ProductBasicWithIdDto> GetById(int enterpriseId, int id);
        public int Create(int enterpriseId, CategoryCoreDto dto);
        public void Update(int enterpriseId, CategoryDto dto);
        public void Delete(int enterpriseId, int id);
    }
}
