/// <summary>
/// Реализация парсера математических выражений по представленной
/// ниже грамматике
/// 
/// BNF:
/// (https://en.wikipedia.org/wiki/Backus%E2%80%93Naur_form)
/// 
/// <code>
/// <expr> ::= <term> + <expr> | <term> - <expr> | <term>
/// <term> ::= <factor> * <term> | <factor> / <term> | <factor>
/// <factor> ::= ( <expr> ) | <number>
/// </code>
/// 
/// </summary>
using CalcCS.Expressions;
using System;

namespace CalcCS.Parser
{
    /// <summary>
    /// Класс-контейнер вспомогательных методов парсера
    /// </summary>
    class ParserImpl
    {
        /// <summary>
        /// Функция пропуска пробелов в исходной строке, начиная с указанной позиции
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция (изменяется)</param>
        public static void SkipSpaces(string source, ref int pos)
        {
            while (pos < source.Length && source[pos] == ' ') pos++;
        }
    }

    /// <summary>
    /// Парсер символа <expr> в представленной грамматике
    /// </summary>
    class ExprParser
    {
        /// <summary>
        /// Метод проверяет применим ли символ <expr> к текущей позиции
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция</param>
        /// <returns>истина, если применим, ложь -- в обратном случае</returns>
        public static bool IsApplicable(string source, int pos)
        {
            return TermParser.IsApplicable(source, pos);
        }

        /// <summary>
        /// Метод выполянет разбор исходной строки, начиная с указанной позиции
        /// и заканчивая концом выражения
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция (изменяется)</param>
        /// <returns>экземпляр Expression</returns>
        /// <exception cref="ParserException" />
        /// <exception cref="InvalidOperationException" />
        public static IExpression Parse(string source, ref int pos)
        {
            IExpression left = TermParser.Parse(source, ref pos);
            ParserImpl.SkipSpaces(source, ref pos);
            if (pos == source.Length) return left;

            if (source[pos] == '+' || source[pos] == '-')
            {
                char op = source[pos];
                pos++;
                ParserImpl.SkipSpaces(source, ref pos);
                if (!ExprParser.IsApplicable(source, pos))
                {
                    throw new ParserException("Invalid expression at " + pos);
                }

                IExpression right = ExprParser.Parse(source, ref pos);

                switch(op)
                {
                    case '+': return new AddOperation(left, right);
                    case '-': return new SubtractOperation(left, right);
                }

                throw new InvalidOperationException("Invalid parser state");
            }

            return left;
        }
    }

    /// <summary>
    /// Парсер символа <term> в представленной грамматике
    /// </summary>
    class TermParser
    {
        /// <summary>
        /// Метод проверяет применим ли символ <term> к текущей позиции
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция</param>
        /// <returns>истина, если применим, ложь -- в обратном случае</returns>
        public static bool IsApplicable(string source, int pos)
        {
            return FactorParser.IsApplicable(source, pos);
        }

        /// <summary>
        /// Метод выполянет разбор исходной строки, начиная с указанной позиции
        /// и заканчивая концом выражения
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция (изменяется)</param>
        /// <returns>экземпляр Expression</returns>
        /// <exception cref="ParserException" />
        /// <exception cref="InvalidOperationException" />
        public static IExpression Parse(string source, ref int pos)
        {
            IExpression left = FactorParser.Parse(source, ref pos);
            ParserImpl.SkipSpaces(source, ref pos);
            if (pos == source.Length) return left;

            if (source[pos] == '*' || source[pos] == '/' || source[pos] == '%')
            {
                char op = source[pos];
                pos++;
                ParserImpl.SkipSpaces(source, ref pos);
                if (!TermParser.IsApplicable(source, pos))
                {
                    throw new ParserException("Invalid term at " + pos);
                }

                IExpression right = TermParser.Parse(source, ref pos);

                switch (op)
                {
                    case '*': return new MultiplyOperation(left, right);
                    case '/': return new DivideOperation(left, right);
                    case '%': return new DivideWithResidue(left, right);
                }

                throw new InvalidOperationException("Invalid parser state");
            }

            return left;
        }
    }

    /// <summary>
    /// Парсер символа <factor> в представленной грамматике
    /// </summary>
    class FactorParser
    {
        /// <summary>
        /// Метод проверяет применим ли символ <factor> к текущей позиции
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция</param>
        /// <returns>истина, если применим, ложь -- в обратном случае</returns>
        public static bool IsApplicable(string source, int pos)
        {
            if (pos >= source.Length) return false;
            return source[pos] == '(' || NumberParser.IsApplicable(source, pos);
        }

        /// <summary>
        /// Метод выполянет разбор исходной строки, начиная с указанной позиции
        /// и заканчивая концом выражения
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция (изменяется)</param>
        /// <returns>экземпляр Expression</returns>
        /// <exception cref="ParserException" />
        public static IExpression Parse(string source, ref int pos)
        {
            if (source[pos] == '(')
            {
                pos++;
                ParserImpl.SkipSpaces(source, ref pos);
                if (!ExprParser.IsApplicable(source, pos))
                {
                    throw new ParserException("Invalid expression at " + pos);
                }

                IExpression expr = ExprParser.Parse(source, ref pos);

                ParserImpl.SkipSpaces(source, ref pos);
                if (pos == source.Length || source[pos] != ')')
                {
                    throw new ParserException("Expected ) at " + pos);
                }

                pos++;
                return expr;
            }

            return NumberParser.Parse(source, ref pos);
        }
    }

    /// <summary>
    /// Парсер символа <number> в представленной грамматике
    /// </summary>
    class NumberParser
    {
        /// <summary>
        /// Метод проверяет применим ли символ <number> к текущей позиции
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция</param>
        /// <returns>истина, если применим, ложь -- в обратном случае</returns>
        public static bool IsApplicable(string source, int pos)
        {
            if (pos >= source.Length) return false;
            return source[pos] >= '0' && source[pos] <= '9';
        }

        /// <summary>
        /// Метод выполянет разбор исходной строки, начиная с указанной позиции
        /// и заканчивая концом выражения
        /// </summary>
        /// <param name="source">строка с исходным математическим выражением</param>
        /// <param name="pos">текущая позиция (изменяется)</param>
        /// <returns>экземпляр Expression</returns>
        /// <exception cref="ParserException" />}
        public static IExpression Parse(string source, ref int pos)
        {
            int start = pos;
            while (IsApplicable(source, pos)) pos++;

            if (pos < source.Length && source[pos] == '.')
            {
                pos++;
                if (!IsApplicable(source, pos))
                {
                    throw new ParserException("Expected digit at " + pos);
                }

                while (IsApplicable(source, pos)) pos++;
            }

            double value = Convert.ToDouble(source.Substring(start, pos - start));
            return new NumberExpr(value);
        }
    }
}
