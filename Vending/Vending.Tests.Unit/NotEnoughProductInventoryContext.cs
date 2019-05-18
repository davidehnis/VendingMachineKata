namespace Vending.Tests.Unit
{
    internal class NotEnoughProductInventoryContext : DefaultContext
    {
        internal NotEnoughProductInventoryContext()
        {
            AvailableBins.Clear();
        }
    }
}