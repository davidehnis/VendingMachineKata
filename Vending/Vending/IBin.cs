using System.Collections.Generic;

namespace Vending
{
    public interface IBin
    {
        string Id { get; }

        IEnumerable<IProduct> Inventory { get; }
    }
}