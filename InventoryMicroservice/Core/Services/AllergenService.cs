﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication;
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
        private readonly IHeaderContextService _headerContextService;

        public AllergenService(ILogger<AllergenService> logger, MicroserviceContext context, IMapper mapper, IBus bus, RabbitMq rabbitMq, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _bus = bus;
            _rabbitMq = rabbitMq;
            _headerContextService = headerContextService;
        }

        public object Get(int enterpriseId)
        {
            var dtos = _context
               .Allergens
               .AsNoTracking()
               .Where(a => a.EspId == enterpriseId)
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

        public AllergenViewModel<ProductBasicWithIdDto> GetById(int enterpriseId, int id)
        {
            var dto = _context
                .Allergens
                .AsNoTracking()
                .Where(a => a.EspId == enterpriseId && a.Id == id)
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

        public async Task<int> Create(int enterpriseId, AllergenCoreDto dto)
        {
            var model = _mapper.Map<Allergen>(dto);
            model.EspId = enterpriseId;
            model.CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);

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
                        message.EudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);
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

        public async Task Update(int enterpriseId, AllergenDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int enterpriseId, int id)
        {
            var model = _context.Allergens
               .FirstOrDefault(a =>
                   a.Id == id &&
                   a.EspId == enterpriseId);

            _context.Allergens.Remove(model);

            var message = new AllergenPayloadValue() { Id = id, EspId = enterpriseId };
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
