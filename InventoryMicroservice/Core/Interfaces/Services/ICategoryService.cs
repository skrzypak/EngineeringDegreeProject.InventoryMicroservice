using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.ViewModel.Category;

namespace InventoryMicroservice.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        public CategoryViewModel Get(int id);
        public int Create(CategoryBasicDto dto);
        public void Update(CategoryBasicDto dto);
        public void Delete(int id);
    }
}
