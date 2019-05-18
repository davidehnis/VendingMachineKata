namespace Vending
{
    public sealed class Metal : IMetal
    {
        public Metal(decimal radius, decimal thickness, decimal weight)
        {
            Radius = radius;
            Thickness = thickness;
            Weight = weight;
        }

        public static IMetal Dime { get; } = new Metal(6, 2, 20);

        public static IMetal Nickel { get; } = new Metal(8, 4, 40);

        public static IMetal Quarter { get; } = new Metal(12, 3, 60);

        public decimal Radius { get; }

        public decimal Thickness { get; }

        public decimal Weight { get; }

        public ICoin Create()
        {
            if (Equals(this, Metal.Nickel)) return new Coin(CoinType.Nickel, new decimal(0.05));
            if (Equals(this, Metal.Dime)) return new Coin(CoinType.Dime, new decimal(0.10));
            if (Equals(this, Metal.Quarter)) return new Coin(CoinType.Quarter, new decimal(0.25));

            return null;
        }

        public bool Equals(IMetal other)
        {
            if (other == null) return false;
            return Radius == other.Radius && Thickness == other.Thickness && Weight == other.Weight; throw new System.NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Metal)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Radius.GetHashCode();
                hashCode = (hashCode * 397) ^ Thickness.GetHashCode();
                hashCode = (hashCode * 397) ^ Weight.GetHashCode();
                return hashCode;
            }
        }

        protected bool Equals(Metal other)
        {
            return Radius == other.Radius && Thickness == other.Thickness && Weight == other.Weight;
        }
    }
}