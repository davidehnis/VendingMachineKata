namespace Vending
{
    public interface IProduct
    {
        decimal Cost { get; }

        ProductType Type { get; }
    }
}