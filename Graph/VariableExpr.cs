using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class VariableExpr : IExpression
    {
        public VariableExpr() { }
        public double Calculate()
        {
            NumbersSingleton nm = NumbersSingleton.GetInstance();
            return nm.GetNumber();
        }
    }
}