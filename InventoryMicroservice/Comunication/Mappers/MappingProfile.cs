using System.Collections.Generic;
using AutoMapper;
using Comunication.Shared;
using Comunication.Shared.PayloadValue;
using InventoryMicroservice.Core.Fluent.Entities;

namespace InventoryMicroservice.Comunication.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Allergen, AllergenPayloadValue>();

            CreateMap<Product, ProductPayloadValue>()
                .ForMember(dest => dest.Allergens, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    using (var enumerator = src.AllergensToProducts.GetEnumerator())
                    {
                        while(enumerator.MoveNext())
                        {
                            dest.Allergens.Add(enumerator.Current.AllergenId, CRUD.Exists);
                        }
                    }

                    using (var enumerator = src.CategoriesToProducts.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            dest.Categories.Add(enumerator.Current.CategoryId, CRUD.Exists);
                        }
                    }
                });
        }
    }
}
