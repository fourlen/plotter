using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class ZeroInZeroPowerException : Exception
    {
        public ZeroInZeroPowerException(string message) : base(message) { }
    }
}
