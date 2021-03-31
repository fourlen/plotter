using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class SinFunction : IExpression
    {
        IExpression arg;
        public SinFunction(IExpression a)
        {
            arg = a;
        }

        public double Calculate()
        {
            return Math.Sin(arg.Calculate());
        }
    }
}
