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
                        .SetAllergens(p.AllergensToProducts.Select(a => new AllergenDto
                            {
                                Id = a.AllergenId,
                                Code = a.Allergen.Code,
                                Name = a.Allergen.Name,
                                Description = a.Allergen.Description,
                            }).ToHashSet())
                        .SetCategories(p.CategoriesToProducts.Select(c => new CategoryDto
                            {
                                Id = c.CategoryId,
                                Code = c.Category.Code,
                                Name = c.Category.Name,
                                Description = c.Category.Description,
                            }).ToHashSet()
                        ).Build()
                    )
                .FirstOrDefault();

            if (productViewModel is null)
            {
                throw new NotFoundException($"Product with ID {id} NOT FOUND");
            }

            return productViewModel;
        }

        public int Create(ProductCreateDto dto)
        {
            var product = new Product
            {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                Unit = dto.Unit,
                AllergensToProducts = new HashSet<AllergenToProduct>(),
                CategoriesToProducts = new HashSet<CategoryToProduct>()
            };

            foreach (var id in dto.AssignAllergens)
            {
                product.AllergensToProducts.Add(new AllergenToProduct { AllergenId = id });
            }

            foreach (var id in dto.AssignCategories)
            {
                product.CategoriesToProducts.Add(new CategoryToProduct
                {
                    CategoryId = id
                });
            }

            foreach (var item in dto.CreateAllergens)
            {
                product.AllergensToProducts.Add(new AllergenToProduct
                {
                    Allergen = _mapper.Map<Allergen>(item)
                });
            }

            foreach (var item in dto.CreateCategories)
            {
                product.CategoriesToProducts.Add(new CategoryToProduct
                {
                    Category = _mapper.Map<Category>(item)
                });
            }


            _context.Products.Add(product);
            _context.SaveChanges();

            return product.Id;
        }

        public void Update(ProductUpdateDto dto)
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
