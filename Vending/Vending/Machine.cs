using System;
using System.Threading.Tasks;

namespace Vending
{
    public class Machine : IMachine
    {
        public Task Boot(IContext initial, Action<IResult, IContext> callback)
        {
            throw new NotImplementedException();
        }

        public Task Insert(IMetal coin, IContext context, Action<IResult, IContext> callback)
        {
            throw new NotImplementedException();
        }

        public Task Return(IContext context, Action<IResult, IContext> callback)
        {
            throw new NotImplementedException();
        }

        public Task Select(string bin, Action<IResult, ISelection> callback)
        {
            throw new NotImplementedException();
        }

        public Task Setup(IContext initial, Action<IResult, IContext> callback)
        {
            throw new NotImplementedException();
        }
    }
}