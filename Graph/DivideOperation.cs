using System;

namespace Graph
{
    /// <summary>
    /// Операция деления
    /// </summary>
    class DivideOperation : BinaryOperation
    {
        /// <summary>
        /// Конструктор
        /// Обычно наследуется без изменений в классах конкретных операций.
        /// </summary>
        /// <param name="left">левый операнд</param>
        /// <param name="right">правый операнд</param>
        public DivideOperation(IExpression left, IExpression right) : base(left, right)
        { }

        /// <summary>
        /// Метод вычисляет численное значение выражения
        /// </summary>
        /// <returns>значение выражения как вещественное число</returns>
        public override double Calculate()
        {
            double numerator = _left.Calculate();
            double denominator = _right.Calculate();
            if (denominator == 0)
            {
                throw new DivideByZeroException("Division by zero");
            }
            return numerator / denominator;
        }
    }
}
