using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vending
{
    public class Machine : IMachine
    {
        public Task Boot(IContext initial, Action<IResult> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                callback?.Invoke(new Result());
            });
        }

        public Task Insert(IMetal metal, IContext context, Action<IResult> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new Result();
                var coin = Factory.Create(metal);
                if (coin == null)
                {
                    context.InvalidCoin(metal);
                    result.Push(new Result(Status.InvalidCoin, false));
                    callback?.Invoke(result);
                }
                else
                {
                    context.ValidCoin(coin);
                    result.Push(new Result(Status.Success, true));
                    callback?.Invoke(result);
                }
            });
        }

        public Task Return(IContext context, Action<IResult> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var coin in context.DepositedCoins.Items)
                {
                    context.CoinReturn.Deposit(coin.ToMetal());
                }

                context.StartState();
                callback?.Invoke(new Result(Status.Success, true));
            });
        }

        public Task Select(IContext context, string bin, Action<IResult> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                var product = context.SelectProduct(bin);
                var isValidBin = context.AvailableBins.Items.FirstOrDefault(b => b.Id == bin) != null;
                if (product == null)
                {
                    context.DisplayMessage.Push(Tags.SoldOut);
                    context.StartState();
                    var res = new Result(Status.SoldOut, true);
                    callback?.Invoke(res);

                    return;
                }

                if (!isValidBin)
                {
                    context.DisplayMessage.Push(Tags.InvalidSelection);
                    context.StartState();
                    var res = new Result(Status.SoldOut, true);
                    callback?.Invoke(res);

                    return;
                }

                context.Purchase(product);
            });
        }
    }
}