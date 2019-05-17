namespace Vending
{
    /// <summary>
    /// Represents the final state/status of a unit of work
    /// </summary>
    public interface IResult
    {
        IStatus Status { get; }

        bool Success();
    }
}