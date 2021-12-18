using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Comunication;
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
        private readonly RabbitMq _rabbitMq;

        public ProductService(ILogger<ProductService> logger, MicroserviceContext context, IMapper mapper, IBus bus, RabbitMq rabbitMq)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _bus = bus;
            _rabbitMq = rabbitMq;
        }

        public object Get(int espId)
        {

            var dtos = _context
               .Products
               .AsNoTracking()
               .Where(p => p.EspId == espId)
               .Select(p => new {
                   p.Id,
                   p.Code,
                   p.Name,
                   p.Unit,
                   p.Description,
                   p.Calories,
               })
               .OrderBy(px => px.Name)
               .ToHashSet();

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any allergen");
            }

            return dtos;
        }

        public ProductViewModel<AllergenDto, CategoryDto> GetById(int espId, int id)
        {
            var productViewModel = _context
                .Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Include(p => p.AllergensToProducts.OrderBy(a2p => a2p.Allergen.Name))
                    .ThenInclude(a2p => a2p.Allergen)
                .Include(p => p.Category)
                .Where(p => p.EspId == espId)
                .Select(p => ProductViewModel<AllergenDto, CategoryDto>.Builder
                        .Id(p.Id)
                        .Code(p.Code)
                        .Name(p.Name)
                        .Unit(p.Unit)
                        .Description(p.Description)
                        .SetAllergens(p.AllergensToProducts.Select(a => new AllergenDto(a)).ToHashSet())
                        .Category(new CategoryDto(p.Category))
                        .Calories(p.Calories)
                        .Proteins(p.Proteins)
                        .Carbohydrates(p.Carbohydrates)
                        .Fats(p.Fats)
                        .Build()
                )
                .FirstOrDefault();

            if (productViewModel is null)
            {
                throw new NotFoundException($"Product with ID {id} NOT FOUND");
            }

            return productViewModel;
        }

        public async Task<int> Create(int espId, int eudId, ProductCoreDto<int , int> dto)
        {
            var model = _mapper.Map<ProductCoreDto<int, int>, Product>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Products.Add(model);
                        await _context.SaveChangesAsync();

                        var message = _mapper.Map<Product, ProductPayloadValue>(model);
                        message.EudId = eudId;
                        await SyncAsync(message, CRUD.Create);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });

            return model.Id;
        }

        public async Task Update(int espId, int eudId, ProductDto<int, int> dto)
        {

            var model = _context.Products.Where(p => p.Id == dto.Id && p.EspId == espId).FirstOrDefault();

            if (model is null)
            {
                throw new NotFoundException($"Product with ID {dto.Id} NOT FOUND");
            }

            var dtomap = _mapper.Map<ProductCoreDto<int, int>, Product>(dto);


            model.Code = dtomap.Code;
            model.Name = dtomap.Name;
            model.Description = dtomap.Description;
            model.Unit = dtomap.Unit;
            model.Category = dtomap.Category;
            model.AllergensToProducts = dtomap.AllergensToProducts;
            model.LastUpdatedEudId = eudId;

            model.Calories = dtomap.Calories;
            model.Proteins = dtomap.Proteins;
            model.Carbohydrates = dtomap.Carbohydrates;
            model.Fats = dtomap.Fats;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Products.Update(model);
                        await _context.SaveChangesAsync();

                        var message = _mapper.Map<Product, ProductPayloadValue>(model);
                        message.EudId = eudId;
                        await SyncAsync(message, CRUD.Update);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            });
        }

        public async Task Delete(int espId, int eudId, int id)
        {
            var model = _context.Products
                .FirstOrDefault(p =>
                    p.Id == id &&
                    p.EspId == espId);

            _context.Products.Remove(model);

            var message = new ProductPayloadValue() { Id = id, EspId = espId, EudId = eudId };
            await SyncAsync(message, CRUD.Delete);

            _context.SaveChanges();
        }

        private async Task SyncAsync(ProductPayloadValue message, CRUD crud)
        {
            var payload = new Payload<ProductPayloadValue>(message, crud);

            Uri[] uri = {
                new Uri($"{_rabbitMq.Host}/msinvo.product.queue"),
                new Uri($"{_rabbitMq.Host}/msgas.product.queue")
            };

            CancellationTokenSource s_cts = new CancellationTokenSource();
            s_cts.CancelAfter(5000);

            try
            {

                foreach (var u in uri)
                {
                    var endPoint = await _bus.GetSendEndpoint(u);
                    await endPoint.Send(payload, s_cts.Token);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                s_cts.Dispose();
            }
        }
    }
}
