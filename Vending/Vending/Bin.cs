using System.Collections.Generic;

namespace Vending
{
    public class Bin : IBin
    {
        public Bin(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public IInventory<IProduct> Inventory => Products;

        protected Inventory<IProduct> Products { get; } = new Inventory<IProduct>();

        public void Add(IProduct product)
        {
            Products.Deposit(product);
        }
    }
}