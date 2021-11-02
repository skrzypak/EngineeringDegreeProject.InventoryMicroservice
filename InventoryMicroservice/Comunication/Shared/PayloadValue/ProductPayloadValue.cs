using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;
using InventoryMicroservice.Core.Models.Dto.Product;

namespace Comunication.Shared.PayloadValue
{
    public class ProductPayloadValue : ProductDto<int, int>, IMessage
    {
    }
}
