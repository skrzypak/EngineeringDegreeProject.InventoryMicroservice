using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Fluent.Entities
{
    public class Allergen : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<AllergenToProduct> AllergensToProducts { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
    }
}
