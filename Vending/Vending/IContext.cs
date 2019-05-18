using System.Collections.Generic;

namespace Vending
{
    /// <summary>Manages the current state of the experience</summary>
    public interface IContext
    {
        IEnumerable<CoinType> AcceptedDenominations { get; }

        IInventory<IBin> AvailableBins { get; }

        IInventory<ICoin> CoinInventory { get; }

        IInventory<IMetal> CoinReturn { get; }

        IInventory<ICoin> DepositedCoins { get; }

        IDisplay DisplayAmount { get; }

        IDisplay DisplayMessage { get; }

        IInventory<IProduct> ProductReturn { get; }

        decimal TotalDeposit { get; }

        IEnumerable<IStatus> Trace { get; }

        /// <summary>
        /// Updates the accepted denominations list
        /// </summary>
        /// <param name="type"></param>
        void Add(CoinType type);

        /// <summary>
        /// Updates the valid and available bins
        /// </summary>
        /// <param name="bin"></param>
        void Add(IBin bin);

        /// <summary>
        /// Inserts a new status to the status trace
        /// </summary>
        /// <param name="status"></param>
        void Add(IStatus status);

        /// <summary>
        /// Returns (removes) all coins from the Coin
        /// Inventory collection
        /// </summary>
        void EjectCoins();

        /// <summary>
        /// Adds a coin to the Coin Inventory collection
        /// </summary>
        /// <param name="coin"></param>
        void Insert(ICoin coin);

        void InvalidCoin(IMetal metal);

        void MadePurchaseState(IProduct product);

        void MakeChange(decimal productCost);

        void NotEnoughCoinsState(IProduct product);

        void Purchase(IProduct product);

        IProduct SelectProduct(string bin);

        void StartState();

        void ValidCoin(ICoin coin);
    }
}