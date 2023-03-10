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
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UnitType Unit { get; set; }
        public Category Category { get; set; }
        public virtual ICollection<AllergenToProduct> AllergensToProducts { get; set; }
        public virtual ICollection<Inventory> AsInventoryItem { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
        public int? Calories { get; set; }
        public float? Proteins { get; set; }
        public float? Carbohydrates { get; set; }
        public float? Fats { get; set; }
    }
}
