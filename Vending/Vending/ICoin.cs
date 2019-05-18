namespace Vending
{
    public interface ICoin : IAsset
    {
        CoinType Type { get; }

        IMetal ToMetal();
    }
}