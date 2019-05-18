using System;

namespace Vending
{
    /// <summary>
    /// The measurements of an inserted piece of metal
    /// </summary>
    public interface IMetal : IAsset, IEquatable<IMetal>
    {
        decimal Radius { get; }

        decimal Thickness { get; }

        decimal Weight { get; }

        ICoin Create();
    }
}