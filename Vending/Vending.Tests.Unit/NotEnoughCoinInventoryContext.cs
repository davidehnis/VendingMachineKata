namespace Vending.Tests.Unit
{
    internal class NotEnoughCoinInventoryContext : DefaultContext
    {
        internal NotEnoughCoinInventoryContext()
        {
            CoinInventoryList.Clear();
        }
    }
}