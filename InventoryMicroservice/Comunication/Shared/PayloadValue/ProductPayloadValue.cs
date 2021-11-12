using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;
using InventoryMicroservice.Core.Models.Dto.Product;

namespace Comunication.Shared.PayloadValue
{
    public class ProductPayloadValue : ProductBasicDto, IMessage
    {
        public int Id { get; set; }
        public IDictionary<int, CRUD> Allergens { get; set; } = new Dictionary<int, CRUD>();
        public int EspId { get; set; }
        public int EudId { get; set; }
    }
}
