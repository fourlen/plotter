namespace Graph
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
        double Calculate();
    }
}
