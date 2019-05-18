using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Vending
{
    public class Context : IContext
    {
        public Context()
        {
            DisplayMessage = new Display();
            DisplayAmount = new Display();
        }

        public IEnumerable<CoinType> AcceptedDenominations => AcceptedDenomitationList;

        public IInventory<IBin> AvailableBins { get; } = new AssetInventory<IBin>();

        public IInventory<ICoin> CoinInventory { get; } = new AssetInventory<ICoin>();

        public IInventory<IMetal> CoinReturn { get; } = new AssetInventory<IMetal>();

        public IInventory<ICoin> DepositedCoins { get; } = new AssetInventory<ICoin>();

        public IDisplay DisplayAmount { get; }

        public IDisplay DisplayMessage { get; protected set; }

        public IInventory<IProduct> ProductReturn { get; } = new AssetInventory<IProduct>();

        public decimal TotalDeposit { get; protected set; }

        public IEnumerable<IStatus> Trace => TraceList;

        protected List<CoinType> AcceptedDenomitationList { get; } = new List<CoinType>();

        protected List<IStatus> TraceList { get; } = new List<IStatus>();

        public void Add(CoinType type)
        {
            AcceptedDenomitationList.Add(type);
        }

        public void Add(IBin bin)
        {
            AvailableBins.Deposit(bin);
        }

        public void Add(IStatus status)
        {
            TraceList.Add(status);
        }

        public void EjectCoins()
        {
            DepositedCoins.Clear();
        }

        public void Insert(ICoin coin)
        {
            DepositedCoins.Deposit(coin);
        }

        public void InvalidCoin(IMetal metal)
        {
            CoinReturn.Deposit(metal);
            StartState();
        }

        public void MadePurchaseState(IProduct product)
        {
            ProductReturn.Deposit(product);
            DisplayMessage.Push("THANK YOU");
            MakeChange(product.Cost);

            StartState();
        }

        public void MakeChange(decimal productCost)
        {
            var coins = new List<ICoin>();

            var cost = productCost;
            var deposited = DepositedCoins.TotalValue();
            var amountOwedToCustomer = deposited - cost;
            var refundRemaining = amountOwedToCustomer;

            if ((amountOwedToCustomer % new decimal(0.25)) < amountOwedToCustomer)
            {
                var numOfCoins = (int)(amountOwedToCustomer / new decimal(0.25));
                for (var i = 0; i < numOfCoins; i++)
                {
                    coins.Add(Coin.Quarter);
                }

                refundRemaining = refundRemaining % new decimal(0.25);
                amountOwedToCustomer = refundRemaining;
            }

            if ((amountOwedToCustomer % new decimal(0.10)) < amountOwedToCustomer)
            {
                var numOfCoins = (int)(amountOwedToCustomer / new decimal(0.10));
                for (var i = 0; i <= numOfCoins; i++)
                {
                    coins.Add(Coin.Dime);
                }

                refundRemaining = refundRemaining % new decimal(0.10);
                amountOwedToCustomer = refundRemaining;
            }

            if ((amountOwedToCustomer % new decimal(0.05)) < amountOwedToCustomer)
            {
                var numOfCoins = (int)(amountOwedToCustomer / new decimal(0.05));
                for (var i = 0; i <= numOfCoins; i++)
                {
                    coins.Add(Coin.Nickel);
                }

                refundRemaining = refundRemaining % new decimal(0.05);
                amountOwedToCustomer = refundRemaining;
            }

            foreach (var coin in coins)
            {
                CoinInventory.Remove(coin);
                CoinReturn.Deposit(coin.ToMetal());
            }
        }

        public void NotEnoughCoinsState(IProduct product)
        {
            DisplayMessage.Push(Tags.Price);
            DisplayAmount.Push($"{product.Cost:C}");

            StartState();
        }

        public void Purchase(IProduct product)
        {
            var depositedCoinTotal
                = DepositedCoins.Items.Select(c => c.Cost).Sum();

            if (depositedCoinTotal >= product.Cost)
            {
                MadePurchaseState(product);
            }
            else
            {
                NotEnoughCoinsState(product);
            }

            StartState();
        }

        public IProduct SelectProduct(string binId)
        {
            var bin = AvailableBins
                .Items.FirstOrDefault(b => b.Id == binId);

            return bin?.Inventory.Debit();
        }

        public void StartState()
        {
            DisplayMessage.Push(
                CoinInventory.TotalValue() < new decimal(0.50)
                    ? Tags.ExactChange
                    : Tags.InsertCoin);
            DisplayAmount.Push($"{TotalDeposit:C}");
        }

        public void ValidCoin(ICoin coin)
        {
            DepositedCoins.Deposit(coin);
            TotalDeposit += coin.Cost;

            StartState();
        }
    }
}