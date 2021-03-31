using System;

namespace CalcCS.Parser
{
    /// <summary>
    /// Исключительная ситуация, описывающая ошибку разбора
    /// математического выражения
    /// </summary>
    class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        { }
    }
}
