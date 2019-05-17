using System.Collections.Generic;

namespace Vending
{
    public class Dispensory<T> : IDispensory<T>
    {
        public Dispensory()
        {

        }

        public IEnumerable<T> Inventory => Items;

        protected List<T> Items { get; } = new List<T>();

        public void Deposit(T item)
        {
            Items.Add(item);
        }
    }
}