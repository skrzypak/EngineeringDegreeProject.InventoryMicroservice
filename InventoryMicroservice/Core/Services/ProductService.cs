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
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public ProductService(ILogger<ProductService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public ProductViewModel<AllergenDto, CategoryDto> Get(int id)
        {
            var productViewModel = _context
                .Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Include(p => p.AllergensToProducts.OrderBy(a2p => a2p.Allergen.Name))
                    .ThenInclude(a2p => a2p.Allergen)
                .Include(p => p.CategoriesToProducts.OrderBy(c => c.Category.Name))
                    .ThenInclude(c2p => c2p.Category)
                    .Select(p => ProductViewModel<AllergenDto, CategoryDto>.Builder
                            .Id(p.Id)
                            .Code(p.Code)
                            .Name(p.Name)
                            .Description(p.Description)
                            .SetAllergens(p.AllergensToProducts.Select(a => new AllergenDto(a)).ToHashSet())
                            .SetCategories(p.CategoriesToProducts.Select(c => new CategoryDto(c)).ToHashSet()
                        ).Build()
                    )
                .FirstOrDefault();

            if (productViewModel is null)
            {
                throw new NotFoundException($"Product with ID {id} NOT FOUND");
            }

            return productViewModel;
        }

        public int Create(ProductDto<int, int> dto)
        {
            var model = _mapper.Map<ProductDto<int, int>, Product>(dto);

            _context.Products.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Update(ProductDto<int, int> dto, ICollection<int> removeAllergensIds, ICollection<int> removeCategoriesIds)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var product = new Product() { Id = id };
            _context.Products.Attach(product);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
