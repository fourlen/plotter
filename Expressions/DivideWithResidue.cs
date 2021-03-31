using System;
using System.Collections.Generic;
using System.Text;

namespace CalcCS.Expressions
{
    class DivideWithResidue : BinaryOperation
    {
        public DivideWithResidue(IExpression left, IExpression right) : base(left, right)
        { }
        public override double Calculate()
        {
            double numerator = _left.Calculate();
            double denominator = _right.Calculate();
            if (denominator == 0)
            {
                throw new DivideByZeroException("Division by zero");
            }
            return numerator % denominator;
        }
    }
}
