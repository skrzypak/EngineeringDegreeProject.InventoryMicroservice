using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Interfaces.ViewModel;
using InventoryMicroservice.Core.Models.Dto.Category;

namespace InventoryMicroservice.Core.Models.ViewModel.Category
{
    public class CategoryViewModel : CategoryBasicDto, IViewModel
    {
        public int Id { get; set; }
    }
}
