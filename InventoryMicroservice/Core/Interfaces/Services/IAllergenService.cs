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
        public int Create(AllergenCoreDto dto);
        public void Update(AllergenDto dto);
        public void Delete(int id);
    }
}
