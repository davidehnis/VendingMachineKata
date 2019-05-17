using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vending
{
    /// <summary>
    /// Represent the user console that shows small messages
    /// and counts
    /// </summary>
    public interface IDisplay
    {
        string Current { get; }

        IEnumerable<string> MessageStack { get; }

        Task Push(string message);
    }
}