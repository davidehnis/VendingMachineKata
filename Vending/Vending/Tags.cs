namespace Vending
{
    public class Tags
    {
        private Tags()
        {
        }

        public static string ExactChange { get; } = "EXACT CHANGE ONLY";

        public static string InsertCoin { get; } = "INSERT COIN";

        public static string InvalidSelection { get; } = "INVALID SELECTION";

        public static string Price { get; } = "PRICE";

        public static string SoldOut { get; } = "SOLD OUT";

        public static string ThankYou { get; } = "THANK YOU";
    }
}