using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Product;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface IProductService
    {
        public ProductViewModel Get(int id);
        public int Create(ProductCreateDto dto);
        public void Update(ProductUpdateDto dto);
        public void Delete(int id);
    }
}
