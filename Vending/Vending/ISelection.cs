using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vending
{
    /// <summary>
    /// Represents the user-initiated selection from the
    /// vending machine interface
    /// </summary>
    public interface ISelection
    {
        /// <summary>The value of the bin selection</summary>
        string BinId { get; }
    }
}