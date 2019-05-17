using System.Collections.Generic;

namespace Vending
{
    public class Context : IContext
    {
        public IEnumerable<CoinType> AcceptedDenominations => AcceptedDenomitationList;

        public IEnumerable<IBin> AvailableBins => AvailableBinList;

        public IEnumerable<ICoin> CoinInventory => CoinInventoryList;

        public IDispensory<ICoin> CoinReturn { get; } = new Dispensory<ICoin>();

        public ISelection CurrentSelection { get; protected set; }

        public IEnumerable<ICoin> DepositedCoins => DepositedCoinList;

        public IDisplay Display { get; protected set; }

        public IEnumerable<IProduct> ProductInventory => ProductInventoryList;

        public IDispensory<IProduct> ProductReturn { get; } = new Dispensory<IProduct>();

        public decimal TotalDeposit { get; protected set; }

        public IEnumerable<IStatus> Trace => TraceList;

        protected List<CoinType> AcceptedDenomitationList { get; } = new List<CoinType>();

        protected List<IBin> AvailableBinList { get; } = new List<IBin>();

        protected List<ICoin> CoinInventoryList { get; } = new List<ICoin>();

        protected List<ICoin> DepositedCoinList { get; } = new List<ICoin>();

        protected List<IProduct> ProductInventoryList { get; } = new List<IProduct>();

        protected List<IStatus> TraceList { get; } = new List<IStatus>();

        public void Add(CoinType type)
        {
            AcceptedDenomitationList.Add(type);
        }

        public void Add(IBin bin)
        {
            AvailableBinList.Add(bin);
        }

        public void Add(IStatus status)
        {
            TraceList.Add(status);
        }

        public void EjectCoins()
        {
            DepositedCoinList.Clear();
        }

        public void Insert(ICoin coin)
        {
            DepositedCoinList.Add(coin);
        }

        public void Update(ISelection selection)
        {
            CurrentSelection = selection;
        }
    }
}