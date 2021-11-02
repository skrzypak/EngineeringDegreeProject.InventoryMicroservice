using System.Collections.Generic;
using AutoMapper;
using Comunication.Shared.PayloadValue;
using InventoryMicroservice.Core.Fluent.Entities;

namespace InventoryMicroservice.Comunication.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductPayloadValue>();
        }
    }
}
