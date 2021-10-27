using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Interfaces.ViewModel;
using InventoryMicroservice.Core.Models.Dto.Category;

namespace InventoryMicroservice.Core.Models.ViewModel.Category
{
    public class CategoryViewModel<T> : CategoryBasicDto, IViewModel
    {
        public static CategoryViewModelBuilder Builder => new();
        public int Id { get; set; }
        public dynamic Products { get; private set; }

        public class CategoryViewModelBuilder
        {
            private int id;
            private string code;
            private string name;
            private string description;
            private dynamic products = new HashSet<T>();

            public CategoryViewModelBuilder Id(int id)
            {
                this.id = id;
                return this;
            }

            public CategoryViewModelBuilder Code(string code)
            {
                this.code = code;
                return this;
            }

            public CategoryViewModelBuilder Name(string name)
            {
                this.name = name;
                return this;
            }

            public CategoryViewModelBuilder Description(string description)
            {
                this.description = description;
                return this;
            }

            public CategoryViewModelBuilder SetProducts(dynamic products)
            {
                this.products = products;
                return this;
            }

            public CategoryViewModelBuilder AddProductItem(T products)
            {
                this.products.Add(products);
                return this;
            }

            public CategoryViewModel<T> Build()
            {
                var item = new CategoryViewModel<T>()
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
