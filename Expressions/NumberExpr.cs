namespace CalcCS.Expressions
{
    /// <summary>
    /// Числовая константа как математическое выражение
    /// </summary>
    class NumberExpr : IExpression
    {
        private double _value;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="value">значение числовой константы</param>
        public NumberExpr(double value)
        {
            _value = value;
        }

        /// <summary>
        /// Метод вычисляет численное значение выражения
        /// </summary>
        /// <returns>значение выражения как вещественное число</returns>
        public double Calculate()
        {
            return _value;
        }
    }
}
