﻿namespace Vending
{
    /// <summary>The associated information of a status</summary>
    public interface IStatus
    {
        string Message { get; }

        StatusType Type { get; }
    }
}