using System;
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
using InventoryMicroservice.Core.Models.Dto.Product;
using InventoryMicroservice.Core.Models.ViewModel.Allergen;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Services
{
    public class AllergenService : IAllergenService
    {
        private readonly ILogger<AllergenService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly RabbitMq _rabbitMq;

        public AllergenService(ILogger<AllergenService> logger, MicroserviceContext context, IMapper mapper, IBus bus, RabbitMq rabbitMq)
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
               .Allergens
               .AsNoTracking()
               .Where(a => a.EspId == espId)
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

        public AllergenViewModel<ProductBasicWithIdDto> GetById(int espId, int id)
        {
            var dto = _context
                .Allergens
                .AsNoTracking()
                .Where(a => a.EspId == espId && a.Id == id)
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

        public async Task<int> Create(int espId, int eudId, AllergenCoreDto dto)
        {
            var model = _mapper.Map<Allergen>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Allergens.Add(model);
                        await _context.SaveChangesAsync();

                        var message = _mapper.Map<Allergen, AllergenPayloadValue>(model);
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

        public async Task Update(int espId, int eudId, AllergenDto dto)
        {
            var model = _context.Allergens.Where(a => a.Id == dto.Id && a.EspId == espId).FirstOrDefault();

            if (model is null)
            {
                throw new NotFoundException($"Allergen with ID {dto.Id} NOT FOUND");
            }

            model.Code = dto.Code;
            model.Name = dto.Name;
            model.Description = dto.Description;
            model.LastUpdatedEudId = eudId;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Allergens.Update(model);
                        await _context.SaveChangesAsync();

                        var message = _mapper.Map<Allergen, AllergenPayloadValue>(model);
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
            var model = _context.Allergens
               .FirstOrDefault(a =>
                   a.Id == id &&
                   a.EspId == espId);

            _context.Allergens.Remove(model);

            var message = new AllergenPayloadValue() { Id = id, EspId = espId };
            await SyncAsync(message, CRUD.Delete);

            _context.SaveChanges();
        }

        private async Task SyncAsync(AllergenPayloadValue message, CRUD crud)
        {
            var payload = new Payload<AllergenPayloadValue>(message, crud);

            Uri[] uri = {
                new Uri($"{_rabbitMq.Host}/msgas.allergen.queue"),
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
