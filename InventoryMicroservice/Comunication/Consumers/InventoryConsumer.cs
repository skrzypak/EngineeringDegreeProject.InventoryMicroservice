using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared;
using Comunication.Shared.PayloadValue;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Comunication.Consumers
{
    public class InventoryConsumer : IConsumer<Payload<InventoryPayloadValue>>
    {
        readonly ILogger<InventoryConsumer> _logger;
        private readonly MicroserviceContext _context;

        public InventoryConsumer(ILogger<InventoryConsumer> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Consume(ConsumeContext<Payload<InventoryPayloadValue>> context)
        {
            _logger.LogInformation("Received Delivered Products data: {Text}", context.Message.Value);

            switch(context.Message.Type)
            {
                case CRUD.Create:
                {
                    Create(context.Message.Value); 
                    break;
                }
                case CRUD.Update:
                {
                    Update(context.Message.Value);
                    break;
                }
            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        private void Create(InventoryPayloadValue val)
        {
            var model = MapToModel(val);
            _context.Inventories.AddRange(model);
        }

        private void Update(InventoryPayloadValue val)
        {
            var model = MapToModel(val);
            _context.Inventories.UpdateRange(model);
        }

        private ICollection<Inventory> MapToModel(InventoryPayloadValue val)
        {
            ICollection<Inventory> inventories = new HashSet<Inventory>();

            foreach (var item in val.Items)
            {
                if (item.Crud == CRUD.Create || item.Crud == CRUD.Update || item.Crud == CRUD.Exists)
                {
                    inventories.Add(new Inventory()
                    {
                        ProductId = item.ProductId,
                        InvoicingSupplierId = val.InvoicingSupplierId,
                        InvoicingDocumentId = val.InvoicingDocumentId,
                        InvoicingDocumentToProductId = item.InvoicingDocumentToProductId,
                        NumOfAvailable = item.NumOfAvailable,
                        ExpirationDate = item.ExpirationDate
                    });
                }
            }

            return inventories;
        }

    }
}
