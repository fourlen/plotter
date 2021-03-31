namespace CalcCS.Expressions
{
    /// <summary>
    /// Общий предок для бинарных операторов
    /// </summary>
    abstract class BinaryOperation : IExpression
    {
        protected IExpression _left;
        protected IExpression _right;

        /// <summary>
        /// Конструктор
        /// Обычно наследуется без изменений в классах конкретных операций.
        /// </summary>
        /// <param name="left">левый операнд</param>
        /// <param name="right">правый операнд</param>
        public BinaryOperation(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public abstract double Calculate();
    }
}
