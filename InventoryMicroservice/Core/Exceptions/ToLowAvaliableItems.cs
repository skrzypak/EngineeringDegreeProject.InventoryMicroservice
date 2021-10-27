using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Exceptions
{
    public class ToLowAvaliableItems : Exception
    {
        public ToLowAvaliableItems(string msg) : base(msg)
        {
        }
    }
}
