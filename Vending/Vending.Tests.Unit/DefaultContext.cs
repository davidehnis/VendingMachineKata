using System.Collections.Generic;
using System.Linq;

namespace Vending.Tests.Unit
{
    internal class DefaultContext : Context
    {
        internal DefaultContext()
        {
            AcceptedDenomitationList.Add(CoinType.Quarter);
            AcceptedDenomitationList.Add(CoinType.Nickel);
            AcceptedDenomitationList.Add(CoinType.Dime);

            AvailableBins.Deposit(CreateDefaultBins());
            CoinInventory.Deposit(CreateDefaultCoinInventory());
            TotalDeposit = 0;
            TraceList.Clear();
        }

        private static IEnumerable<IBin> CreateDefaultBins()
        {
            var results = new List<IBin>();
            var binA = new Bin("A");
            binA.Add(new Product(new decimal(1.00), ProductType.Cola));
            binA.Add(new Product(new decimal(1.00), ProductType.Cola));
            binA.Add(new Product(new decimal(1.00), ProductType.Cola));
            binA.Add(new Product(new decimal(1.00), ProductType.Cola));
            binA.Add(new Product(new decimal(1.00), ProductType.Cola));

            var binB = new Bin("B");
            binB.Add(new Product(new decimal(0.50), ProductType.Chips));
            binB.Add(new Product(new decimal(0.50), ProductType.Chips));
            binB.Add(new Product(new decimal(0.50), ProductType.Chips));

            var binC = new Bin("C");
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));
            binC.Add(new Product(new decimal(0.65), ProductType.Candy));

            results.Add(binA);
            results.Add(binB);
            results.Add(binC);

            return results;
        }

        private static IEnumerable<ICoin> CreateDefaultCoinInventory()
        {
            var results = new List<ICoin>();

            var nickels = Enumerable.Repeat<ICoin>(Coin.Nickel, 50).ToList();
            var dimes = Enumerable.Repeat<ICoin>(Coin.Dime, 50).ToList();
            var quarters = Enumerable.Repeat<ICoin>(Coin.Dime, 80).ToList();

            results.AddRange(nickels);
            results.AddRange(dimes);
            results.AddRange(quarters);

            return results;
        }
    }
}