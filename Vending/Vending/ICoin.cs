namespace Vending
{
    public interface ICoin
    {
        Coin Type { get; }

        decimal Value { get; }
    }
}