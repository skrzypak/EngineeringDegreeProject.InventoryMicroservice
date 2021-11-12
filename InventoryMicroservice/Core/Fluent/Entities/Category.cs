using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Fluent.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public int EspId { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }

    }
}
