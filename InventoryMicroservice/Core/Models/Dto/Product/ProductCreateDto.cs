using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;

namespace InventoryMicroservice.Core.Models.Dto.Product
{
    public class ProductCreateDto : ProductBasicDto
    {
        public ICollection<int> AssignAllergens { get; set; }
        public ICollection<int> AssignCategories { get; set; }
        public ICollection<AllergenDto> CreateAllergens { get; set; }
        public ICollection<CategoryDto> CreateCategories { get; set; }
    }
}
