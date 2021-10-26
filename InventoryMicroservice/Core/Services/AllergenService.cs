﻿using System;
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

        public AllergenViewModel<ProductDto> Get(int id)
        {
            var allergenViewModel = _context
                .Allergens
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Include(a2p => a2p.AllergensToProducts.OrderBy(a => a.Product.Name))
                    .ThenInclude(a2p => a2p.Product)
                .Select(c => AllergenViewModel<ProductDto>.Builder
                    .Id(c.Id)
                    .Code(c.Code)
                    .Name(c.Name)
                    .Description(c.Description)
                    .SetProducts(c.AllergensToProducts.Select(p => new ProductDto
                    {
                        Id = p.Product.Id,
                        Name = p.Product.Name,
                        Code = p.Product.Code,
                        Description = p.Product.Description,
                        Unit = p.Product.Unit
                    }).ToHashSet())
                    .Build()
                 )
                .FirstOrDefault();

            if (allergenViewModel is null)
            {
                throw new NotFoundException($"Allergen with ID {id} NOT FOUND");
            }

            return allergenViewModel;
        }

        public int Create(AllergenBasicDto dto)
        {
            var category = _mapper.Map<Allergen>(dto);
            _context.Allergens.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public void Update(AllergenDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var category = new Allergen() { Id = id };
            _context.Allergens.Attach(category);
            _context.Allergens.Remove(category);
            _context.SaveChanges();
        }

    }
}