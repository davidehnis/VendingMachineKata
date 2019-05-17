using System.Collections.Generic;

namespace Vending
{
    public class Bin : IBin
    {
        public Bin(string id)
        {
            Id = id;
        }

        protected List<IProduct> Products { get; } = new List<IProduct>();

        public void Add(IProduct product)
        {
            Products.Add(product);
        }

        public string Id { get; }
        public IEnumerable<IProduct> Inventory => Products;
    }
}