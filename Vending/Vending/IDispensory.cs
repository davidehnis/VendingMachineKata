using System.Collections.Generic;

namespace Vending
{
    public interface IDispensory<T>
    {
        IEnumerable<T> Inventory { get; }

        void Deposit(T product);
    }
}