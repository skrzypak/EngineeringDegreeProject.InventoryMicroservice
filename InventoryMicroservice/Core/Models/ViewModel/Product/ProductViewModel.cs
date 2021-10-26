using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;
using InventoryMicroservice.Core.Interfaces.ViewModel;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;

namespace InventoryMicroservice.Core.Models.ViewModel.Product
{
    public class ProductViewModel : ProductBasicDto, IViewModel
    {
        public static ProductViewModelBuilder Builder => new ProductViewModelBuilder();
        public int Id { get; private set; }
        public ICollection<AllergenDto> Allergens { get; private set; }
        public ICollection<CategoryDto> Categories { get; private set; }

        public class ProductViewModelBuilder
        {
            private int id;
            private string code;
            private string name;
            private string description;
            private UnitType unit;
            private ICollection<AllergenDto> allergens = new HashSet<AllergenDto>();
            private ICollection<CategoryDto> categories = new HashSet<CategoryDto>();

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

            public ProductViewModelBuilder Descripton(string description)
            {
                this.description = description;
                return this;
            }

            public ProductViewModelBuilder Unit(UnitType unit)
            {
                this.unit = unit;
                return this;
            }

            public ProductViewModelBuilder SetAllergens(ICollection<AllergenDto> allergens)
            {
                this.allergens = allergens;
                return this;
            }

            public ProductViewModelBuilder AddAllergenItem(AllergenDto allergen)
            {
                this.allergens.Add(allergen);
                return this;
            }

            public ProductViewModelBuilder SetCategories(ICollection<CategoryDto> categories)
            {
                this.categories = categories;
                return this;
            }

            public ProductViewModelBuilder AddCategoryItem(CategoryDto category)
            {
                this.categories.Add(category);
                return this;
            }

            public ProductViewModel build()
            {
                var item = new ProductViewModel();
                item.Id = this.id;
                item.Code = this.code;
                item.Name = this.name;
                item.Description = this.description;
                item.Unit = this.unit;
                item.Allergens = this.allergens;
                item.Categories = this.categories;
                return item;
            }
        }
    }
}
