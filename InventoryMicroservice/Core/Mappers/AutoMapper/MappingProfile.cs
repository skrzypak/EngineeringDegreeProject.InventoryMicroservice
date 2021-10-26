using AutoMapper;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;

namespace InventoryMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AllergenDto, Allergen>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
