using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Models.Dto.Product
{
    public class ProductUpdateDto : ProductCreateDto
    {
        public int Id { get; set; }
        public ICollection<int> RemoveAllergens { get; set; }
        public ICollection<int> RemoveCategories { get; set; }
    }
}
