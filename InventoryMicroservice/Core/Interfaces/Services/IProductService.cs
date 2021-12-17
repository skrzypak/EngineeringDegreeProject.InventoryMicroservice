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
        public object Get(int espId);
        public ProductViewModel<AllergenDto, CategoryDto> GetById(int espId, int id);
        public Task<int> Create(int espId, int eudId, ProductCoreDto<int, int> dto);
        public Task Update(int espId, int eudId, ProductDto<int, int> dto);
        public Task Delete(int espId, int eudId, int id);
    }
}
