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
                case CRUD.Delete:
                {
                    Delete(context.Message.Value);
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
            foreach (var item in val.Items)
            {
                switch(item.Crud)
                {
                    case CRUD.Create:
                    {
                        var model = CreateItem(val.SupplierId, val.DocumentId, val.EspId, item);
                        model.EspId = val.EspId;
                        model.CreatedEudId = val.EudId;
                        _context.Inventories.Add(model);
                        break;
                    }
                    case CRUD.Update:
                    {
                        throw new NotImplementedException();
                    }
                    case CRUD.Delete:
                    {
                        var model = _context.Inventories.Where(i =>
                            i.EspId == val.EspId &&
                            i.SupplierId == val.SupplierId &&
                            i.DocumentId == val.DocumentId &&
                            i.DocumentToProductId == item.DocumentToProductId
                        ).FirstOrDefault();

                        if(model is not null)
                        {
                            _context.Inventories.Remove(model);
                        }
                        break;
                    }
                }
            }
        }

        private void Delete(InventoryPayloadValue val)
        {
            var model = _context.Inventories.Where(i => 
                i.SupplierId == val.SupplierId &&
                i.DocumentId == val.DocumentId
            ).ToList();

            if(model is not null && model.Count > 0)
            {
                _context.Inventories.RemoveRange(model);
            }
        }

        private ICollection<Inventory> MapToModel(InventoryPayloadValue val)
        {
            ICollection<Inventory> inventories = new HashSet<Inventory>();

            foreach (var item in val.Items)
            {
                if (item.Crud == CRUD.Create)
                {
                    var i = CreateItem(val.SupplierId, val.DocumentId, val.EspId, item);
                    i.CreatedEudId = val.EudId;
                    inventories.Add(i);
                }
            }

            return inventories;
        }

        private Inventory CreateItem(int InvoicingSupplierId, int InvoicingDocumentId, int EspId, InventoryPayloadValue.ItemsPayloadValue item)
        {
            return new Inventory()
            {
                ProductId = item.ProductId,
                SupplierId = InvoicingSupplierId,
                DocumentId = InvoicingDocumentId,
                DocumentToProductId = item.DocumentToProductId,
                NumOfAvailable = item.Quantity,
                ExpirationDate = item.ExpirationDate,
                UnitMeasureValue = item.UnitMeasureValue,
                UnitNetPrice = item.UnitNetPrice,
                Quantity = item.Quantity,
                PercentageVat = item.PercentageVat,
                GrossValue = item.GrossValue,
                EspId = EspId,
                NumOfSettled = 0,
                NumOfSpoiled = 0,
                NumOfDamaged = 0
            };
        }

    }
}
