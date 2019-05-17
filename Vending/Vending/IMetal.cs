using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vending
{
    /// <summary>
    /// The measurements of an inserted piece of metal
    /// </summary>
    public interface IMetal
    {
        decimal Radius { get; }

        decimal Thickness { get; }

        decimal Weight { get; }
    }
}