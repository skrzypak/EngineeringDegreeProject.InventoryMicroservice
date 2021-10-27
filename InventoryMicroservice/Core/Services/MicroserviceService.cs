using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryMicroservice.Core.Exceptions;
using InventoryMicroservice.Core.Fluent;
using InventoryMicroservice.Core.Fluent.Entities;
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

        public object GetAvaliableInventoryProducts()
        {
            var categoryViewModelWithInventoryItems = _context
                .Categories
                .AsNoTracking()
                .Include(c => c.CategoriesToProducts.OrderBy(c2p => c2p.Product.Name))
                    .ThenInclude(a2p => a2p.Product)
                        .ThenInclude(p => p.AsInventoryItem)
                            .ThenInclude(iv => iv.InventoryOperations)
                .Select(c => new
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description,
                    Products = c.CategoriesToProducts
                        .Select(c2p => new { 
                            Id = c2p.Product.Id,
                            Code = c2p.Product.Code,
                            Name = c2p.Product.Name,
                            Description = c2p.Product.Description,
                            Avaliable = c2p.Product.AsInventoryItem.Sum(iv => iv.NumOfAvailable),
                            Settled = c2p.Product.AsInventoryItem.Sum(iv => iv.NumOfSettled),
                            Spoiled = c2p.Product.AsInventoryItem.Sum(iv => iv.NumOfSpoiled),
                            Operations = c2p.Product.AsInventoryItem.SelectMany(iv => iv.InventoryOperations
                                .Select(ivo => new
                                {
                                    Id = ivo.Id,
                                    Quantity = ivo.Quantity,
                                    Operation = ivo.Operation,
                                    Date = ivo.Date
                                })
                            )
                            .OrderByDescending(ivox => ivox.Id)
                            .AsEnumerable(),
                            ExpirationDates = c2p.Product.AsInventoryItem.Select(iv => new
                                {
                                    Id = iv.Id,
                                    Avaliable = iv.NumOfAvailable,
                                    Date = iv.ExpirationDate != null ? iv.ExpirationDate : System.DateTime.MaxValue
                            })
                            .OrderBy(ivx => ivx.Date)
                            .AsEnumerable()
                        })
                    .AsEnumerable()
                    .Where(c2px => c2px.Avaliable > 0)
                    .OrderBy(c2px => c2px.Name.ToUpper())
                    .DefaultIfEmpty()
                })
                .AsEnumerable()
                .Where(cx => cx.Products.Count() > 0)
                .OrderBy(cx => cx.Name)
                .ToHashSet();

            if (categoryViewModelWithInventoryItems is null)
            {
                throw new NotFoundException($"Inventory is empty");
            }

            return categoryViewModelWithInventoryItems;
        }

        public void UpdateQuantityInventoryProduct(int productId, ushort toSettling, ushort toSpoilling)
        {
            var productInfo = _context.Products
                .Include(p => p.AsInventoryItem)
                    .Select(p => new
                    {
                        Id = p.Id,
                        Avaliable = p.AsInventoryItem != null ? p.AsInventoryItem.Sum(iv => iv.NumOfAvailable) : 0,
                        Items = p.AsInventoryItem.Select(iv => new
                        {
                            Id = iv.Id,
                            Avaliable = iv.NumOfAvailable
                        })
                        .Where(ivx => ivx.Avaliable > 0)
                        .OrderBy(ivx => ivx.Id) // (dev ID, production EXPIRATION_DATE)
                        .AsEnumerable(),
                    })
                    .Where(px => px.Id == productId)
                .FirstOrDefault();

            if (productInfo is null)
            {
                throw new NotFoundException($"Product with id {productInfo.Id} NOT FOUND");
            }

            ushort numOfItemsToRelease = (ushort)(toSettling + toSpoilling);

            if (productInfo.Avaliable < numOfItemsToRelease)
            {
                throw new ToLowAvaliableItems($"Not enough products available ({productInfo.Id}) in stock");
            }

            // Logged operations
            ICollection<InventoryOperation> inventoryOperations = new List<InventoryOperation>();
            ushort x = 0;

            // FIFO - first transfered item of product is seetling or spoilling
            foreach (var ivItem in productInfo.Items)
            {
                var itemModel = _context.Inventories.First(iv => iv.Id == ivItem.Id);

                if(toSettling > 0)
                {
                    if(itemModel.NumOfAvailable < toSettling)
                    {
                        // Not enought avaliable quantity to end settling in this loop
                        x =  (ushort)(itemModel.NumOfAvailable);
                        toSettling -= x;
                       
                    } else
                    {
                        // Enought avaliable quantity to end settling in this loop
                        x = (ushort)(toSettling);
                        toSettling = 0;
                    }

                    itemModel.NumOfSettled += x;
                    itemModel.NumOfAvailable -= x;

                    inventoryOperations.Add(new InventoryOperation()
                    {
                        InventoryId = ivItem.Id,
                        Quantity = x,
                        Operation = Fluent.Enums.InventoryOperationType.Settle,
                        Description = "SYSTEM",
                        Date = DateTime.Now
                    });

                    if (itemModel.NumOfAvailable <= 0)
                    {
                        itemModel.NumOfAvailable = 0;
                        continue;
                    }
                }

                if(toSpoilling > 0)
                {
                    if (itemModel.NumOfAvailable < toSpoilling)
                    {
                        // Not enought avaliable quantity to end spoilling in this loop
                        x = (ushort)(itemModel.NumOfAvailable);
                        toSpoilling -= x;
                    }
                    else
                    {
                        // Enought avaliable quantity to end spoilling in this loop
                        x = (ushort)(toSpoilling);
                        toSpoilling = 0;
                    }

                    itemModel.NumOfSpoiled += x;
                    itemModel.NumOfAvailable -= x;

                    inventoryOperations.Add(new InventoryOperation()
                    {
                        InventoryId = ivItem.Id,
                        Quantity = x,
                        Operation = Fluent.Enums.InventoryOperationType.Spoile,
                        Description = "SYSTEM",
                        Date = DateTime.Now
                    });
                }

            }

            _context.InventoriesOperations.AddRange(inventoryOperations);

            _context.SaveChanges();
        }
    }
}
