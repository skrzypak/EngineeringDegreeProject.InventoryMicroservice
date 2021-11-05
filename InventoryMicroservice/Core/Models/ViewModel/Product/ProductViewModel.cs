using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;
using InventoryMicroservice.Core.Interfaces.ViewModel;
using InventoryMicroservice.Core.Models.Dto.Product;

namespace InventoryMicroservice.Core.Models.ViewModel.Product
{
    public class ProductViewModel<TA, TC> : ProductCoreDto<TA, TC>, IViewModel
    {
        public static ProductViewModelBuilder Builder => new();
        public int Id { get; private set; }

        public class ProductViewModelBuilder
        {
            private int id;
            private string code;
            private string name;
            private string description;
            private UnitType unit;
            private ICollection<TA> allergens = new HashSet<TA>();
            private TC category;

            public ProductViewModelBuilder Id(int id)
            {
                this.id = id;
                return this;
            }

            public ProductViewModelBuilder Code(string code)
            {
                this.code = code;
                return this;
            }

            public ProductViewModelBuilder Name(string name)
            {
                this.name = name;
                return this;
            }

            public ProductViewModelBuilder Description(string description)
            {
                this.description = description;
                return this;
            }

            public ProductViewModelBuilder Unit(UnitType unit)
            {
                this.unit = unit;
                return this;
            }

            public ProductViewModelBuilder SetAllergens(ICollection<TA> allergens)
            {
                this.allergens = allergens;
                return this;
            }

            public ProductViewModelBuilder AddAllergenItem(TA allergen)
            {
                this.allergens.Add(allergen);
                return this;
            }

            public ProductViewModelBuilder Category(TC category)
            {
                this.category = category;
                return this;
            }

            public ProductViewModel<TA , TC> Build()
            {
                var item = new ProductViewModel<TA, TC>()
                {
                    Id = this.id,
                    Code = this.code,
                    Name = this.name,
                    Description = this.description,
                    Unit = this.unit,
                    Allergens = this.allergens,
                    Category = this.category
                };
                return item;
            }
        }
    }
}
