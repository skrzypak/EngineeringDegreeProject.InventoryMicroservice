using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Comunication.Shared;
using Comunication.Shared.PayloadValue;
using InventoryMicroservice.Core.Exceptions;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Interfaces.Services;
using InventoryMicroservice.Core.Models.Dto.Allergen;
using InventoryMicroservice.Core.Models.Dto.Category;
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public ProductService(ILogger<ProductService> logger, MicroserviceContext context, IMapper mapper, IBus bus)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _bus = bus;
        }

        public object Get()
        {
            var dtos = _context
               .Products
               .AsNoTracking()
               .Select(p => new {
                   p.Id,
                   p.Code,
                   p.Name,
                   p.Description
               })
               .OrderBy(px => px.Name)
               .ToHashSet();

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any allergen");
            }

            return dtos;
        }

        public ProductViewModel<AllergenDto, CategoryDto> GetById(int id)
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

        public async Task<int> Create(ProductCoreDto<int , int> dto)
        {
            var model = _mapper.Map<ProductCoreDto<int, int>, Product>(dto);

            _context.Products.Add(model);
            _context.SaveChanges();

            var message = _mapper.Map<Product, ProductPayloadValue>(model);
            await SyncAsync(message, CRUD.Create);

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

        private async Task SyncAsync(ProductPayloadValue message, CRUD crud)
        {
            var payload = new Payload<ProductPayloadValue>(message, crud);

            Uri[] uri = {
                new Uri("rabbitmq://localhost/msinvo.product.queue"),
                new Uri("rabbitmq://localhost/msgas.product.queue")
            };

            foreach (var u in uri)
            {
                var endPoint = await _bus.GetSendEndpoint(u);
                await endPoint.Send(payload);
            }
        }
    }
}
