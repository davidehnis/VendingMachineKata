namespace Vending
{
    public class Coin : ICoin
    {
        public Coin(CoinType type, decimal value)
        {
            Type = type;
            Value = value;
        }

        public static ICoin Dime { get; }
            = new Coin(CoinType.Dime, new decimal(0.10));

        public static ICoin Nickel { get; }
            = new Coin(CoinType.Nickel, new decimal(0.05));

        public static ICoin Quarter { get; }
            = new Coin(CoinType.Quarter, new decimal(0.10));

        public decimal Cost => Value;

        public CoinType Type { get; }

        public decimal Value { get; }

        public IMetal ToMetal()
        {
            if (this == Quarter) return Metal.Quarter;
            if (this == Nickel) return Metal.Nickel;
            if (this == Dime) return Metal.Dime;

            return null;
        }
    }
}