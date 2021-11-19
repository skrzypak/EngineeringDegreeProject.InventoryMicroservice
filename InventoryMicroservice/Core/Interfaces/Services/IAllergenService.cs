using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Allergen;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IAllergenService
    {
        public object Get(int espId);
        public AllergenViewModel<ProductBasicWithIdDto> GetById(int espId, int id);
        public Task<int> Create(int espId, int eudId, AllergenCoreDto dto);
        public Task Update(int espId, int eudId, AllergenDto dto);
        public Task Delete(int espId, int eudId, int id);
    }
}
