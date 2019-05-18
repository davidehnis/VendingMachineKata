namespace Vending
{
    public interface IProduct : IAsset
    {
        ProductType Type { get; }
    }
}