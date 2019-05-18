using System;
using System.Threading.Tasks;

namespace Vending
{
    /// <summary>
    /// Represents the vending machine interface from the POV
    /// of the user
    /// </summary>
    public interface IMachine
    {
        /// <summary>Initialization sequence</summary>
        /// <param name="initial"> </param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task Boot(IContext initial, Action<IResult> callback);

        /// <summary>Insert metal</summary>
        /// <param name="coin">    </param>
        /// <param name="context"> </param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task Insert(IMetal coin, IContext context, Action<IResult> callback);

        /// <summary>Return coins</summary>
        /// <param name="context"> </param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task Return(IContext context, Action<IResult> callback);

        /// <summary>Make a product selection</summary>
        /// c
        /// <param name="context"> </param>
        /// <param name="bin">     </param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task Select(IContext context, string bin, Action<IResult> callback);
    }
}