using System.Collections.Generic;

namespace Vending
{
    public interface IBin
    {
        string Id { get; }

        IInventory<IProduct> Inventory { get; }
    }
}