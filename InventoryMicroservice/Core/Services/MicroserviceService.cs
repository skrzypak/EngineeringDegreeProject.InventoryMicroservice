using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using InventoryMicroservice.Core.Exceptions;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
using InventoryMicroservice.Core.Fluent.Enums;
using InventoryMicroservice.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Services
{
    public class MicroserviceService : IMicroserviceService
    {
        private readonly ILogger<MicroserviceService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public MicroserviceService(ILogger<MicroserviceService> logger, MicroserviceContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public object GetAvaliableInventoryItems(int espId)
        {
            var dto = _context.Inventories
                .AsNoTracking()
                .Include(iv => iv.Product)
                    .ThenInclude(c => c.Category)
                .Where(iv => iv.EspId == espId)
                .Select(iv => new
                {
                    InventoryId = iv.Id,
                    ProductId = iv.Product.Id,
                    ProductName = iv.Product.Name,
                    ProductCode = iv.Product.Code,
                    ProductDescription = iv.Product.Description,
                    ProductUnit = iv.Product.Unit,
                    UnitMeasureValue = iv.UnitMeasureValue,
                    NumOfAvailable = iv.NumOfAvailable,
                    Category = iv.Product.Category,
                    iv.ExpirationDate
                }).ToList().Where(ivx => ivx.NumOfAvailable > 0).GroupBy(ivx => new { ivx.Category.Id, ivx.Category.Code, ivx.Category.Name }).Select(ivg => new
                {
                    ivg.Key,
                    Products = ivg.Select(g => new {
                        g.InventoryId,
                        g.ProductId,
                        g.ProductCode,
                        g.ProductName,
                        g.ProductDescription,
                        g.UnitMeasureValue,
                        g.NumOfAvailable,
                        g.ProductUnit,
                        g.ExpirationDate
                    }).ToList().GroupBy(px => new { px.ProductId, px.ProductCode, px.ProductName, px.ProductDescription, px.ProductUnit }).Select(pg => new
                    {
                        pg.Key,
                        Items = pg.Select(pg => new { 
                            InventoryId = pg.InventoryId,
                            NumOfAvailable = pg.NumOfAvailable,
                            UnitMeasureValue = pg.UnitMeasureValue,
                            pg.ExpirationDate
                        }).ToList().GroupBy(ix => new { ix.UnitMeasureValue }).Select(ixg => new
                        {
                            ixg.Key,
                            ItemsInventoryIds = ixg.Select(g => new { 
                                g.InventoryId,
                                g.NumOfAvailable,
                                g.ExpirationDate
                            }),
                            TotalNumOfAvailable = ixg.Sum(g => g.NumOfAvailable)
                        }),
                    }),
                }).ToList();

            if (dto is null)
            {
                throw new NotFoundException($"NOT FOUND avaliable inventory items");
            }

            return dto;
        }

        public object GetInventorySummary(int espId, DateTime startDate, DateTime endDate) 
        {
            var dtos = _context.Inventories
                .AsNoTracking()
                .Include(iv => iv.Product)
                .Include(iv => iv.InventoryOperations)
                .Where(iv => iv.EspId == espId)
                .Select(iv => new
                {
                    ProductId = iv.ProductId,
                    Name = iv.Product.Name,
                    Code = iv.Product.Code,
                    Description = iv.Product.Description,
                    UnitMeasureValue = iv.UnitMeasureValue,
                    Unit = iv.Product.Unit,
                    SettledItem = iv.InventoryOperations
                        .Where(ivo => ivo.InventoryId == iv.Id)
                        .Where(ivo => ivo.Operation == InventoryOperationType.Settle)
                        .Where(ivo => startDate <= ivo.Date && ivo.Date <= endDate)
                        .Sum(ivo => ivo.Quantity),
                    SpoiledItem = iv.InventoryOperations
                        .Where(ivo => ivo.InventoryId == iv.Id)
                        .Where(ivo => ivo.Operation == InventoryOperationType.Spoile)
                        .Where(ivo => startDate <= ivo.Date && ivo.Date <= endDate)
                        .Sum(ivo => ivo.Quantity),
                    DamagedItem = iv.InventoryOperations
                        .Where(ivo => ivo.InventoryId == iv.Id)
                        .Where(ivo => ivo.Operation == InventoryOperationType.Damage)
                        .Where(ivo => startDate <= ivo.Date && ivo.Date <= endDate)
                        .Sum(ivo => ivo.Quantity),
                    NetPriceItem = iv.InventoryOperations
                        .Where(ivo => ivo.InventoryId == iv.Id)
                        .Where(ivo => ivo.Operation == InventoryOperationType.Settle || ivo.Operation == InventoryOperationType.Spoile || ivo.Operation == InventoryOperationType.Damage)
                        .Where(ivo => startDate <= ivo.Date && ivo.Date <= endDate)
                        .Sum(ivo => ivo.Quantity * ivo.Inventory.UnitNetPrice),
                    GrossValueItem = iv.InventoryOperations
                        .Where(ivo => ivo.InventoryId == iv.Id)
                        .Where(ivo => ivo.Operation == InventoryOperationType.Settle || ivo.Operation == InventoryOperationType.Spoile || ivo.Operation == InventoryOperationType.Damage)
                        .Where(ivo => startDate <= ivo.Date && ivo.Date <= endDate)
                        .Sum(ivo => ivo.Quantity * (decimal)(ivo.Inventory.GrossValue / ivo.Inventory.Quantity)),
                }).ToList().Where(ivx => ivx.SettledItem > 0 || ivx.SpoiledItem > 0 || ivx.DamagedItem > 0).GroupBy(ivx => new { ivx.ProductId, ivx.Name, ivx.Code, ivx.Unit, ivx.Description }).Select(ivg => new
                {
                    ivg.Key,
                    Items = ivg.Select(g => new
                    {
                        g.UnitMeasureValue,
                        g.SettledItem,
                        g.SpoiledItem,
                        g.DamagedItem,
                        g.NetPriceItem,
                        g.GrossValueItem
                    }).GroupBy(gx => new { gx.UnitMeasureValue }).Select(gxg => new {
                        gxg.Key,
                        TotalSeetledItems = gxg.Sum(g => g.SettledItem),
                        TotalSpoiledItem = gxg.Sum(g => g.SpoiledItem),
                        TotalDamagedItem = gxg.Sum(g => g.DamagedItem),
                        TotalNetPriceItem = gxg.Sum(g => g.NetPriceItem),
                        TotalGrossValueItem = gxg.Sum(g => g.GrossValueItem)
                    })
                }).ToList().OrderBy(l => l.Key.Name);

            return dtos;
        }

        public void UpdateInventoryItemManual(int espId, int eudId, int id, InventoryOperationType operationType, ushort quantity)
        {
            var item = _context.Inventories.FirstOrDefault(iv => iv.EspId == espId && iv.Id == id);
            item.LastUpdatedEudId = eudId;

            if (item is null)
            {
                throw new NotFoundException($"Item with id {id} NOT FOUND");
            }


            if (item.NumOfAvailable < quantity)
            {
                throw new ToLowAvaliableItems($"Not enough products items available ({id}) in stock");
            }

            item.NumOfAvailable -= quantity;

            var operationLog = new InventoryOperation()
            {
                InventoryId = id,
                Quantity = quantity,
                Description = "USER",
                Date = DateTime.Now,
                EspId = espId,
                CreatedEudId = eudId
            };

            switch (operationType)
            {
                case InventoryOperationType.Settle:
                    {
                        item.NumOfSettled += quantity;
                        operationLog.Operation = InventoryOperationType.Settle;
                        break;
                    }
                case InventoryOperationType.Spoile:
                    {
                        item.NumOfSpoiled += quantity;
                        operationLog.Operation = InventoryOperationType.Spoile;
                        break;
                    }
                case InventoryOperationType.Damage:
                    {
                        item.NumOfDamaged += quantity;
                        operationLog.Operation = InventoryOperationType.Damage;
                        break;
                    }
                default:
                    {
                        throw new Exception("Invalid type of operation");
                    }
            }

            _context.InventoriesOperations.Add(operationLog);

            _context.SaveChanges();
        }

        public void UpdateInventoryProduct(int espId, int eudId, int productId, InventoryOperationType operationType, ushort quantity, ushort unitMeasureValue)
        {
            var matchingItems = _context.Products
                .AsNoTracking()
                .Include(p => p.AsInventoryItem)
                .Where(p => p.EspId == espId && p.Id == productId)
                .SelectMany(p => 
                    p.AsInventoryItem.Select(iv => new {
                        iv.Id,
                        iv.NumOfAvailable,
                        iv.UnitMeasureValue,
                    })
                    .Where(ivx => (ushort)ivx.UnitMeasureValue == unitMeasureValue && ivx.NumOfAvailable > 0)
                    .AsEnumerable()
                    .OrderBy(ivx => ivx.Id)
                )
                .ToList();

            if (matchingItems is null)
            {
                throw new NotFoundException($"NOT FOUND any matching items for product with id {productId}, umv: {unitMeasureValue}");
            }

            var sumOfAvaliable = matchingItems.Sum(mp => mp.NumOfAvailable);

            if (quantity > sumOfAvaliable)
            {
                throw new ToLowAvaliableItems($"Not enough divided items available in stock for automatic process");
            }

            ICollection<InventoryOperation> inventoryOperationsLogs = new List<InventoryOperation>();

            ushort todo = quantity;
            ushort processing = 0;

            // FIFO - first transfered item of product is seetling or spoilling
            foreach (var item in matchingItems)
            {
                if (todo > 0)
                {
                    var model = _context.Inventories.First(iv => iv.Id == item.Id);
                    model.LastUpdatedEudId = eudId;

                    if (model.NumOfAvailable < todo)
                    {
                        // Not enought avaliable items to end processing
                        processing = model.NumOfAvailable;
                        todo -= processing;
                    }
                    else
                    {
                        // Enought avaliable items to end processing
                        processing = todo;
                        todo = 0;
                    }

                    // TODO: strategy pattern
                    switch(operationType)
                    {
                        case InventoryOperationType.Settle:
                            {
                                model.NumOfSettled += processing;
                                break;
                            }
                        case InventoryOperationType.Spoile:
                            {
                                model.NumOfSpoiled += processing;
                                break;
                            }
                        case InventoryOperationType.Damage:
                            {
                                model.NumOfDamaged += processing;
                                break;
                            }
                    }

                    model.NumOfAvailable -= processing;

                    inventoryOperationsLogs.Add(new InventoryOperation()
                    {
                        InventoryId = model.Id,
                        Quantity = processing,
                        Operation = operationType,
                        Description = "USER/SYSTEM/AUTO",
                        Date = DateTime.Now,
                        EspId = espId,
                        CreatedEudId = eudId
                    });

                } else
                {
                    break;
                }
            }

            _context.InventoriesOperations.AddRange(inventoryOperationsLogs);

            _context.SaveChanges();
        }
    }
}
