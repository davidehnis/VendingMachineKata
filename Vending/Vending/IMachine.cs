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
        Task Boot(IContext initial, Action<IResult, IContext> callback);

        /// <summary>Insert metal</summary>
        /// <param name="coin">    </param>
        /// <param name="context"> </param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task Insert(IMetal coin, IContext context, Action<IResult, IContext> callback);

        /// <summary>Return coins</summary>
        /// <param name="context"> </param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task Return(IContext context, Action<IResult, IContext> callback);

        /// <summary>Make a product selection</summary>
        /// <param name="bin">     </param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task Select(string bin, Action<IResult, ISelection> callback);
    }
}