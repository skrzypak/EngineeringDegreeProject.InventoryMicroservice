using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication;
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
        private readonly IHeaderContextService _headerContextService;

        public CategoryService(ILogger<CategoryService> logger, MicroserviceContext context, IMapper mapper, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _headerContextService = headerContextService;
        }

        public object Get(int enterpriseId)
        {
            var dtos = _context
               .Categories
               .AsNoTracking()
               .Where(c => c.EspId == enterpriseId)
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

        public CategoryViewModel<ProductBasicWithIdDto> GetById(int enterpriseId, int id)
        {
            var categoryViewModel = _context
                .Categories
                .AsNoTracking()
                .Where(c => c.EspId == enterpriseId && c.Id == id)
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

        public int Create(int enterpriseId, CategoryCoreDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.EspId = enterpriseId;
            category.CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public void Update(int enterpriseId, CategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int enterpriseId, int id)
        {
            var model = _context.Categories
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.EspId == enterpriseId);

            _context.Categories.Remove(model);
            _context.SaveChanges();
        }

    }
}
