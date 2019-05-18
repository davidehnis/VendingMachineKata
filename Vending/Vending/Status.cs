namespace Vending
{
    public class Status : IStatus
    {
        public Status(StatusType type, string message = "")
        {
            Type = type;
            Message = message;
        }

        public static IStatus InvalidCoin { get; } = new Status(StatusType.InvalidCoin);

        public static IStatus New { get; } = new Status(StatusType.New);

        public static IStatus SoldOut { get; } = new Status(StatusType.SoldOut);

        public static IStatus Success { get; } = new Status(StatusType.Success);

        public static IStatus ThankYou { get; } = new Status(StatusType.ThankYou);

        public static IStatus Price { get; } = new Status(StatusType.Price);

        public string Message { get; }

        public StatusType Type { get; }
    }
}