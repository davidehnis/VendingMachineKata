namespace Vending
{
    public class Product : IProduct
    {
        public Product(decimal cost, ProductType type)
        {
            Cost = cost;
            Type = type;
        }

        public decimal Cost { get; }

        public ProductType Type { get; }
    }
}