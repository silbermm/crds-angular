using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossroads.Utilities.Functions
{
    public static class Functions
    {
        public static int IntegerReturnValue(Func<int> func)
        {
            return func();
        }

    }
}
