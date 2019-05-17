using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Vending
{
    public class Context : IContext
    {
        public Context()
        {
            Display = new Display();
        }

        public IEnumerable<CoinType> AcceptedDenominations => AcceptedDenomitationList;

        public IInventory<IBin> AvailableBins => AvailableBinList;

        public IInventory<ICoin> CoinInventory => CoinInventoryList;

        public IInventory<IMetal> CoinReturn { get; } = new Inventory<IMetal>();

        public ISelection CurrentSelection { get; protected set; }

        public IInventory<ICoin> DepositedCoins => DepositedCoinList;

        public IDisplay Display { get; protected set; }

        public IInventory<IProduct> ProductInventory => ProductInventoryList;

        public IInventory<IProduct> ProductReturn { get; } = new Inventory<IProduct>();

        public decimal TotalDeposit { get; protected set; }

        public IEnumerable<IStatus> Trace => TraceList;

        protected List<CoinType> AcceptedDenomitationList { get; } = new List<CoinType>();

        protected Inventory<IBin> AvailableBinList { get; } = new Inventory<IBin>();

        protected Inventory<ICoin> CoinInventoryList { get; } = new Inventory<ICoin>();

        protected Inventory<ICoin> DepositedCoinList { get; } = new Inventory<ICoin>();

        protected Inventory<IProduct> ProductInventoryList { get; } = new Inventory<IProduct>();

        protected List<IStatus> TraceList { get; } = new List<IStatus>();

        public void Add(CoinType type)
        {
            AcceptedDenomitationList.Add(type);
        }

        public void Add(IBin bin)
        {
            AvailableBinList.Deposit(bin);
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
            DepositedCoinList.Deposit(coin);
        }

        public void InvalidCoin(IMetal metal)
        {
            CoinReturn.Deposit(metal);
            Display.Push(TotalDeposit == 0
                ? "INSERT COIN"
                : TotalDeposit.ToString(CultureInfo.InvariantCulture));
        }

        public void Purchase(IProduct product)
        {
            var depositedCoinTotal
                = DepositedCoins.Items.Select(c => c.Value).Sum();

            if (depositedCoinTotal >= product.Cost)
            {
                ProductReturn.Deposit(product);
                Display.Push("THANK YOU");
                Display.Push("INSERT COIN");
                Display.Push("$0.00");

                var clearedCoins = new List<ICoin>();

                var cost = product.Cost;
                var total = new decimal(0.00);
                foreach (var coin in DepositedCoins.Items)
                {
                    var remaining = cost - total;
                    if (remaining <= 0) break;
                    total = total + coin.Value;
                    var cn = DepositedCoins.Debit();
                    clearedCoins.Add(cn);
                }

                foreach (var coin in clearedCoins)
                {
                    CoinReturn.Deposit(coin);
                }

                var result = new Result(Status.ThankYou, true);
                callback?.Invoke(result, context);
            }
        }

        public IProduct SelectProduct(string binId)
        {
            var bin = AvailableBinList
                .Items.FirstOrDefault(b => b.Id == binId);

            return bin?.Inventory.Debit();
        }

        public void Update(ISelection selection)
        {
            CurrentSelection = selection;
        }

        public void ValidCoin(ICoin coin)
        {
            DepositedCoinList.Deposit(coin);
            TotalDeposit += coin.Value;
            Display.Push(TotalDeposit.ToString(CultureInfo.InvariantCulture));
        }
    }
}