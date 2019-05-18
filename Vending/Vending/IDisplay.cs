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

    public class Display : IDisplay
    {
        public Display()
        {
        }

        public string Current { get; protected set; } = string.Empty;

        public IEnumerable<string> MessageStack => Messages;

        protected Stack<string> Messages { get; } = new Stack<string>();

        public Task Push(string message)
        {
            return Task.Factory.StartNew(() =>
            {
                Current = message;
                Messages.Push(message);
            });
        }
    }
}