using System;
using System.Linq;
using AutoMapper;
using InventoryMicroservice.Core.Exceptions;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Interfaces.Services;
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

        public object Get(int espId)
        {
            var dtos = _context
               .Categories
               .AsNoTracking()
               .Where(c => c.EspId == espId)
               .Select(c => new {
                   c.Id,
                   c.Code,
                   c.Name,
                   c.Description
               })
               .OrderBy(cx => cx.Name)
               .ToHashSet();

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any category");
            }

            return dtos;
        }

        public CategoryViewModel<ProductBasicWithIdDto> GetById(int espId, int id)
        {
            var categoryViewModel = _context
                .Categories
                .AsNoTracking()
                .Where(c => c.EspId == espId && c.Id == id)
                .Include(c => c.Products)
                .Select(c => CategoryViewModel<ProductBasicWithIdDto>.Builder
                    .Id(c.Id)
                    .Code(c.Code)
                    .Name(c.Name)
                    .Description(c.Description)
                    .SetProducts(c.Products.Select(p => new ProductBasicWithIdDto(p)).ToList())
                    .Build()
                 )
                .FirstOrDefault();

            if (categoryViewModel is null)
            {
                throw new NotFoundException($"Category with ID {id} NOT FOUND");
            }

            return categoryViewModel;
        }

        public int Create(int espId, int eudId, CategoryCoreDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.EspId = espId;
            category.CreatedEudId = eudId;

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public void Update(int espId, int eudId, CategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int espId, int eudId, int id)
        {
            var model = _context.Categories
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.EspId == espId);

            _context.Categories.Remove(model);
            _context.SaveChanges();
        }

    }
}
