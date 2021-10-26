using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;
using InventoryMicroservice.Core.Interfaces.ViewModel;
using InventoryMicroservice.Core.Models.Dto.Product;

namespace InventoryMicroservice.Core.Models.ViewModel.Product
{
    public class ProductViewModel<TA, TC> : ProductBasicDto, IViewModel
    {
        public static ProductViewModelBuilder Builder => new();
        public int Id { get; private set; }
        public ICollection<TA> Allergens { get; private set; }
        public ICollection<TC> Categories { get; private set; }

        public class ProductViewModelBuilder
        {
            private int id;
            private string code;
            private string name;
            private string description;
            private UnitType unit;
            private ICollection<TA> allergens = new HashSet<TA>();
            private ICollection<TC> categories = new HashSet<TC>();

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

            public ProductViewModelBuilder SetCategories(ICollection<TC> categories)
            {
                this.categories = categories;
                return this;
            }

            public ProductViewModelBuilder AddCategoryItem(TC category)
            {
                this.categories.Add(category);
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
                    Categories = this.categories
                };
                return item;
            }
        }
    }
}
