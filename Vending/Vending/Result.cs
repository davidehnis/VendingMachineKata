using System.Collections.Generic;

namespace Vending
{
    public class Result : IResult
    {
        public Result()
        {
            SuccessValue = true;
            Current = Status.New;
        }

        public Result(IStatus status, bool success)
        {
            SuccessValue = success;
            Current = status;
        }

        public IStatus Current { get; }

        protected List<IResult> CompositeResults { get; } = new List<IResult>();

        protected bool SuccessValue { get; set; }

        public void Push(IResult result)
        {
            CompositeResults.Add(result);
        }

        public bool Success()
        {
            return SuccessValue &&
                   CompositeResults.TrueForAll(c => c.Success());
        }
    }
}