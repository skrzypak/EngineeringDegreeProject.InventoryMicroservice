using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Allergen;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IAllergenService
    {
        public object Get();
        public AllergenViewModel<ProductBasicWithIdDto> GetById(int id);
        public Task<int> Create(AllergenCoreDto dto);
        public Task Update(AllergenDto dto);
        public Task Delete(int id);
    }
}
