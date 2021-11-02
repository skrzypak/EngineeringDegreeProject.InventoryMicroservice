using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;
using InventoryMicroservice.Core.Models.Dto.Allergen;

namespace Comunication.Shared.Messages
{
    public class AllergenPayloadValue : AllergenDto, IMessage
    {
    }
}
