using AutoMapper;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;

namespace InventoryMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Allergen, AllergenDto>();
            CreateMap<AllergenDto, Allergen>();
            CreateMap<Allergen, AllergenBasicDto>();
            CreateMap<AllergenBasicDto, Allergen>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryBasicDto>();
            CreateMap<CategoryBasicDto, Category>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductBasicDto>();
            CreateMap<ProductBasicDto, Product>();
        }
    }
}
