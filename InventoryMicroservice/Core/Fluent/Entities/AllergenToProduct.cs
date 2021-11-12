using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Fluent.Entities
{
    public class AllergenToProduct : IEntity
    {
        public int AllergenId { get; set; }
        public virtual Allergen Allergen { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
    }
}
