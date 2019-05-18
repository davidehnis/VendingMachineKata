namespace Vending
{
    public interface ICoin
    {
        CoinType Type { get; }

        decimal Value { get; }

        IMetal ToMetal();
    }
}