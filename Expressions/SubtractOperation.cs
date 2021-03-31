namespace CalcCS.Expressions
{
    /// <summary>
    /// Операция вычитания
    /// </summary>
    class SubtractOperation : BinaryOperation
    {
        /// <summary>
        /// Конструктор
        /// Обычно наследуется без изменений в классах конкретных операций.
        /// </summary>
        /// <param name="left">левый операнд</param>
        /// <param name="right">правый операнд</param>
        public SubtractOperation(IExpression left, IExpression right) : base(left, right)
        { }

        /// <summary>
        /// Метод вычисляет численное значение выражения
        /// </summary>
        /// <returns>значение выражения как вещественное число</returns>
        public override double Calculate()
        {
            return _left.Calculate() - _right.Calculate();
        }
    }
}
