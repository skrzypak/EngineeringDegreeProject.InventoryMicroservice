using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Allergen;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IAllergenService
    {
        public object Get(int enterpriseId);
        public AllergenViewModel<ProductBasicWithIdDto> GetById(int enterpriseId, int id);
        public Task<int> Create(int enterpriseId, AllergenCoreDto dto);
        public Task Update(int enterpriseId, AllergenDto dto);
        public Task Delete(int enterpriseId, int id);
    }
}
