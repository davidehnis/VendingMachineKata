namespace Vending
{
    public class Metal : IMetal
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
    }
}