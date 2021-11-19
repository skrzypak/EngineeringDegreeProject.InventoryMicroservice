using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Category;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        public object Get(int espId);
        public CategoryViewModel<ProductBasicWithIdDto> GetById(int espId, int id);
        public int Create(int espId, int eudId, CategoryCoreDto dto);
        public void Update(int espId, int eudId, CategoryDto dto);
        public void Delete(int espId, int eudId, int id);
    }
}
