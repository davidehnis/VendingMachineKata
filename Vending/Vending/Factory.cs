using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vending
{
    public class Factory
    {
        private Factory()
        {
        }

        public static ICoin Create(IMetal metal)
        {
            return !Rules.MetalMustBeValid(metal)
                ? null
                : metal.Create();
        }
    }

    public class Rules
    {
        public static bool MetalMustBeValid(IMetal metal)
        {
            if (metal == null) return false;

            return Equals(metal, Metal.Nickel) ||
                   Equals(metal, Metal.Dime) ||
                   Equals(metal, Metal.Quarter);
        }
    }
}