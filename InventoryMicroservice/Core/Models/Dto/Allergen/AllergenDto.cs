using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Entities;

namespace InventoryMicroservice.Core.Models.Dto.Allergen
{
    public class AllergenDto : AllergenCoreDto
    {
        public int Id { get; set; }

        public AllergenDto(AllergenToProduct values)
        {
            Id = values.AllergenId;
            Code = values.Allergen.Code;
            Name = values.Allergen.Name;
            Description = values.Allergen.Description;
        }
    }
}
