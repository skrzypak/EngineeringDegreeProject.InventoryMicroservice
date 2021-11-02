using System.Collections.Generic;
using AutoMapper;
using Comunication.Shared;
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
            CreateMap<Allergen, AllergenCoreDto>();
            CreateMap<AllergenCoreDto, Allergen>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryCoreDto>();
            CreateMap<CategoryCoreDto, Category>();

            CreateMap<ProductCoreDto<int, int>, Product>()
                .ForMember(dest => dest.AllergensToProducts, opt => opt.Ignore())
                .ForMember(dest => dest.CategoriesToProducts, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.AllergensToProducts = new HashSet<AllergenToProduct>();
                    dest.CategoriesToProducts = new HashSet<CategoryToProduct>();

                    foreach (var id in src.Allergens)
                    {
                        dest.AllergensToProducts.Add(new AllergenToProduct { AllergenId = id });
                    }

                    foreach (var id in src.Categories)
                    {
                        dest.CategoriesToProducts.Add(new CategoryToProduct { CategoryId = id });
                    }
                });
        }
    }
}
