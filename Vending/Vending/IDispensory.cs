using System.Collections.Generic;

namespace Vending
{
    public interface IDispensory
    {
        IEnumerable<IProduct> Inventory { get; }

        void Deposit(IProduct product);
    }
}