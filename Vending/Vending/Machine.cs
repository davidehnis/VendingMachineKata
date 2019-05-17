using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vending
{
    public class Machine : IMachine
    {
        public Task Boot(IContext initial, Action<IResult, IContext> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                callback?.Invoke(new Result(), initial);
            });
        }

        public Task Insert(IMetal metal, IContext context, Action<IResult, IContext> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new Result();
                var coin = Factory.Create(metal);
                if (coin == null)
                {
                    context.InvalidCoin(metal);
                    result.Push(new Result(Status.InvalidCoin, false));
                    callback?.Invoke(result, context);
                }
                else
                {
                    context.ValidCoin(coin);
                    result.Push(new Result(Status.Success, true));
                    callback?.Invoke(result, context);
                }
            });
        }

        public Task Return(IContext context, Action<IResult, IContext> callback)
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task Select(IContext context, string bin, Action<IResult, IContext> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                var product = context.SelectProduct(bin);
                if (product == null)
                {
                    var res = new Result(Status.SoldOut, false);
                    callback?.Invoke(res, context);
                    return;
                }

                var depositedCoinTotal = context.DepositedCoins.Items.Select(c => c.Value).Sum();
                if (depositedCoinTotal >= product.Cost)
                {
                    context.ProductReturn.Deposit(product);
                    context.Display.Push("THANK YOU");
                    context.Display.Push("INSERT COIN");
                    context.Display.Push("$0.00");
                    context.Purchase(product);
                    var result = new Result(Status.ThankYou, true);
                    callback?.Invoke(result, context);
                }
            });
        }
    }
}