using System.Collections.Generic;

namespace Vending
{
    public class Context : IContext
    {
        public IEnumerable<Coin> AcceptedDenominations => AcceptedDenomitationList;

        public IEnumerable<IBin> AvailableBins => AvailableBinList;

        public IEnumerable<ICoin> CoinInventory => CoinInventoryList;

        public ISelection CurrentSelection { get; protected set; }

        public IEnumerable<ICoin> DepositedCoins => DepositedCoinList;

        public IDisplay Display { get; protected set; }

        public IEnumerable<IStatus> Trace => TraceList;

        protected List<Coin> AcceptedDenomitationList { get; } = new List<Coin>();

        protected List<IBin> AvailableBinList { get; } = new List<IBin>();

        protected List<ICoin> CoinInventoryList { get; } = new List<ICoin>();

        protected List<ICoin> DepositedCoinList { get; } = new List<ICoin>();

        protected List<IStatus> TraceList { get; } = new List<IStatus>();

        public void Add(Coin type)
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