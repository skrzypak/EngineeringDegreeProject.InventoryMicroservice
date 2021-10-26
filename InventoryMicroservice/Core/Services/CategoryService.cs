using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryMicroservice.Core.Exceptions;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Mappers;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ILogger<CategoryService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public CategoryViewModel<ProductDto> Get(int id)
        {
            var categoryViewModel = _context
                .Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Include(c => c.CategoriesToProducts.OrderBy(c2p => c2p.Product.Name))
                    .ThenInclude(a2p => a2p.Product)
                .Select(c => CategoryViewModel<ProductDto>.Builder
                    .Id(c.Id)
                    .Code(c.Code)
                    .Name(c.Name)
                    .Description(c.Description)
                    .SetProducts(c.CategoriesToProducts.Select(p => new ProductDto
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

            if (categoryViewModel is null)
            {
                throw new NotFoundException($"Category with ID {id} NOT FOUND");
            }

            return categoryViewModel;
        }

        public int Create(CategoryBasicDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public void Update(CategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var category = new Category() { Id = id };
            _context.Categories.Attach(category);
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

    }
}
