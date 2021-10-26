using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Interfaces.ViewModel;
using InventoryMicroservice.Core.Models.Dto.Allergen;

namespace InventoryMicroservice.Core.Models.ViewModel.Allergen
{
    public class AllergenViewModel<T> : AllergenBasicDto, IViewModel
    {
        public static AllergenViewModelBuilder Builder => new();
        public int Id { get; set; }
        public ICollection<T> Products { get; private set; }

        public class AllergenViewModelBuilder
        {
            private int id;
            private string code;
            private string name;
            private string description;
            private ICollection<T> products = new HashSet<T>();

            public AllergenViewModelBuilder Id(int id)
            {
                this.id = id;
                return this;
            }

            public AllergenViewModelBuilder Code(string code)
            {
                this.code = code;
                return this;
            }

            public AllergenViewModelBuilder Name(string name)
            {
                this.name = name;
                return this;
            }

            public AllergenViewModelBuilder Description(string description)
            {
                this.description = description;
                return this;
            }

            public AllergenViewModelBuilder SetProducts(ICollection<T> products)
            {
                this.products = products;
                return this;
            }

            public AllergenViewModelBuilder AddProductItem(T products)
            {
                this.products.Add(products);
                return this;
            }

            public AllergenViewModel<T> Build()
            {
                var item = new AllergenViewModel<T>()
                {
                    Id = this.id,
                    Code = this.code,
                    Name = this.name,
                    Description = this.description,
                    Products = this.products
                };
                return item;
            }
        }
    }
}
