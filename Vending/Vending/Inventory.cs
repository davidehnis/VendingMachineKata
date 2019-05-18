using System;
using System.Collections.Generic;
using System.Linq;

namespace Vending
{
    public class Inventory<T> : IInventory<T>
    {
        public IEnumerable<T> Items => ItemList;

        protected List<T> ItemList { get; } = new List<T>();

        public void Clear()
        {
            ItemList.Clear();
        }

        public void Clear(Action<IEnumerable<T>> action)
        {
            action?.Invoke(Items);
        }

        public T Debit()
        {
            if (!ItemList.Any()) return default(T);
            var item = ItemList[0];
            ItemList.Remove(item);
            return item;
        }

        public void Deposit(T item)
        {
            ItemList.Add(item);
        }

        public void Deposit(IEnumerable<T> items)
        {
            ItemList.AddRange(items);
        }
    }
}