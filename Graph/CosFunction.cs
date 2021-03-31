using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class CosFunction : IExpression
    {
        IExpression arg;
        public CosFunction(IExpression a)
        {
            arg = a;
        }

        public double Calculate()
        {
            return Math.Cos(arg.Calculate());
        }
    }
}
