using System.Linq;

namespace Vending
{
    public class Bin : IBin
    {
        public Bin(string id)
        {
            Id = id;
        }

        public decimal Cost => Products.Items.Sum(p => p.Cost);

        public string Id { get; }

        public IInventory<IProduct> Inventory => Products;

        protected IInventory<IProduct> Products { get; }
            = new AssetInventory<IProduct>();

        public void Add(IProduct product)
        {
            Products.Deposit(product);
        }
    }
}