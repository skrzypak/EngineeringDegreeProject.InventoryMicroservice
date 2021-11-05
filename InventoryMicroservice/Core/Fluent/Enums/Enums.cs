using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Core.Fluent.Enums
{
    public enum UnitType
    {
        mg,
        g,
        dag,
        kg,
        ml,
        l,
        piece
    }

    public enum InventoryOperationType
    {
        Add,
        Settle,
        Spoile,
        Damage,
        Remove
    }

}
