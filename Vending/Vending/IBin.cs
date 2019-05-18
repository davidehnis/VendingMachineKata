using System.Collections.Generic;

namespace Vending
{
    public interface IBin : IAsset
    {
        string Id { get; }

        IInventory<IProduct> Inventory { get; }
    }
}