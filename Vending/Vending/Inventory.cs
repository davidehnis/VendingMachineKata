using System;
using System.Collections.Generic;
using System.Linq;

namespace Vending
{
    public class AssetInventory<T> : Inventory<T> where T : IAsset
    {
        public AssetInventory()
        {
            CalcTotalValue = Sum;
        }

        private decimal Sum()
        {
            var assets = Items.Cast<IAsset>();
            return assets.Sum(a => a.Cost);
        }
    }

    public class Inventory<T> : IInventory<T>
    {
        public Inventory(Func<decimal> calcTotalValue)
        {
            CalcTotalValue = calcTotalValue;
        }

        protected Inventory()
        {
        }

        public IEnumerable<T> Items => ItemList;

        protected Func<decimal> CalcTotalValue { get; set; }

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

        public void Remove(T item)
        {
            ItemList.Remove(item);
        }

        public decimal TotalValue()
        {
            return CalcTotalValue?.Invoke() ?? 0;
        }
    }

    public class SummarizedInventory<T> : Inventory<T>
    {
        public SummarizedInventory(Func<decimal> calcTotalValue)
            : base(calcTotalValue)
        {
        }
    }

    public class UnsummarizedInventory<T> : Inventory<T>
    {
        public UnsummarizedInventory()
            : base(() => new decimal(0.00))
        {
        }
    }
}