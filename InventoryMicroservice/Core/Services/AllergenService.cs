using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryMicroservice.Core.Exceptions;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Allergen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Services
{
    public class AllergenService : IAllergenService
    {
        private readonly ILogger<AllergenService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public AllergenService(ILogger<AllergenService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public object Get()
        {
            var dtos = _context
               .Allergens
               .AsNoTracking()
               .Select(a => new {
                    a.Id,
                    a.Code,
                    a.Name,
                    a.Description
               })
               .OrderBy(ax => ax.Name)
               .ToHashSet();

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any allergen");
            }

            return dtos;
        }

        public AllergenViewModel<ProductBasicWithIdDto> GetById(int id)
        {
            var dto = _context
                .Allergens
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Include(a2p => a2p.AllergensToProducts.OrderBy(a => a.Product.Name))
                    .ThenInclude(a2p => a2p.Product)
                .Select(c => AllergenViewModel<ProductBasicWithIdDto>.Builder
                    .Id(c.Id)
                    .Code(c.Code)
                    .Name(c.Name)
                    .Description(c.Description)
                    .SetProducts(c.AllergensToProducts.Select(p => new ProductBasicWithIdDto(p)).ToHashSet())
                    .Build()
                 )
                .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Allergen with ID {id} NOT FOUND");
            }

            return dto;
        }

        public int Create(AllergenCoreDto dto)
        {
            var model = _mapper.Map<Allergen>(dto);
            _context.Allergens.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Update(AllergenDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var model = new Allergen() { Id = id };
            _context.Allergens.Attach(model);
            _context.Allergens.Remove(model);
            _context.SaveChanges();
        }

    }
}
