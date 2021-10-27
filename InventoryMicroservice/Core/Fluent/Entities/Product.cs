using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace InventoryMicroservice.Core.Fluent.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UnitType Unit { get; set; }
        public virtual ICollection<AllergenToProduct> AllergensToProducts { get; set; }
        public virtual ICollection<CategoryToProduct> CategoriesToProducts { get; set; }
        public virtual ICollection<Inventory> AsInventoryItem { get; set; }
    }
}
