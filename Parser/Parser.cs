using CalcCS.Expressions;

namespace CalcCS.Parser
{
    /// <summary>
    /// Класс разбора математического выражения как строки
    /// </summary>
    class Parser
    {
        private string _source;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="source">строка, содержащая математическое выражение</param>
        public Parser(string source)
        {
            _source = source;
        }

        /// <summary>
        /// Метод преобразует строку, переданную в конструктор,
        /// в дерево операндов и возвращает указатель на корневой
        /// элемент.
        /// </summary>
        /// <returns>корневой элемент дерева операндов</returns>
        /// <see cref="Parser(string)"/>
        public IExpression Parse()
        {
            int pos = 0;
            ParserImpl.SkipSpaces(_source, ref pos);
            if (!ExprParser.IsApplicable(_source, pos))
            {
                throw new ParserException("Not a valid expression");
            }

            IExpression result = ExprParser.Parse(_source, ref pos);

            ParserImpl.SkipSpaces(_source, ref pos);
            if (pos != _source.Length)
            {
                throw new ParserException("Unexpected symbol at the end of expression after " + pos);
            }

            return result;
        }
    }
}
