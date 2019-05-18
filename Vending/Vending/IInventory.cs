using System;
using System.Collections.Generic;

namespace Vending
{
    public interface IInventory<T>
    {
        IEnumerable<T> Items { get; }

        void Clear();

        void Clear(Action<IEnumerable<T>> action);

        T Debit();

        void Deposit(T item);

        void Deposit(IEnumerable<T> items);
    }
}