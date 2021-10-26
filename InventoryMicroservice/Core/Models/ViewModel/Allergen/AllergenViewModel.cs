using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Interfaces.ViewModel;
using InventoryMicroservice.Core.Models.Dto.Allergen;

namespace InventoryMicroservice.Core.Models.ViewModel.Allergen
{
    public class AllergenViewModel : AllergenBasicDto, IViewModel
    {
        public int Id { get; set; }
    }
}
