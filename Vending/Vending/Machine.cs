using System;
using System.Globalization;
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
            return Task.Factory.StartNew(() =>
            {
                foreach (var coin in context.DepositedCoins.Items)
                {
                    context.CoinReturn.Deposit(coin.ToMetal());
                }

                return new Result(Status.Success, true);
            });
        }

        public Task Select(IContext context, string bin, Action<IResult, IContext> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                var product = context.SelectProduct(bin);
                if (product == null)
                {
                    context.Display.Push(Tags.Sold_Out);
                    context.Display.Push(Tags.Insert_Coin);
                    var res = new Result(Status.SoldOut, false);
                    callback?.Invoke(res, context);

                    return;
                }

                var depositedCoinTotal = context.DepositedCoins.Items.Select(c => c.Value).Sum();
                if (depositedCoinTotal >= product.Cost)
                {
                    context.ProductReturn.Deposit(product);
                    context.Display.Push(Tags.Thank_You);
                    context.Display.Push(Tags.Insert_Coin);
                    context.Display.Push("$0.00");

                    var result = new Result(Status.Price, false);
                    callback?.Invoke(result, context);
                }
                else
                {
                    context.Display.Push(Tags.Price);
                    var formatted = $"{product.Cost:C}";
                    context.Display.Push(formatted);

                    var result = new Result(Status.ThankYou, true);
                    callback?.Invoke(result, context);
                }
            });
        }
    }
}