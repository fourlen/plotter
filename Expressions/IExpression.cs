namespace CalcCS.Expressions
{
    /// <summary>
    /// Общий предок всех математических выражений
    /// </summary>
    interface IExpression
    {
        /// <summary>
        /// Метод вычисляет численное значение выражения
        /// </summary>
        /// <returns>значение выражения как вещественное число</returns>
        public abstract double Calculate();
    }
}
