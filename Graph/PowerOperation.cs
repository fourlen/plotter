using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class PowerOperation : BinaryOperation
    {
        public PowerOperation(IExpression left, IExpression right) : base(left, right)
        { }
        public override double Calculate()
        {
            double numerator = _left.Calculate();
            double denominator = _right.Calculate();
            if (numerator == 0 && denominator == 0)
            {
                throw new ZeroInZeroPowerException("0^0 is not determined");
            }
            return Math.Pow(numerator, denominator);
        }
    }
}
