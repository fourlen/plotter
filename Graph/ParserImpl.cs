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
using System;

namespace Graph
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
            return PowerParser.IsApplicable(source, pos);
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
            IExpression left = PowerParser.Parse(source, ref pos);
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


    class PowerParser
    {
        public static bool IsApplicable(string source, int pos)
        {
            if (pos >= source.Length) return false;
            return FactorParser.IsApplicable(source, pos);
        }

        public static IExpression Parse(string source, ref int pos)
        {
            IExpression left = FactorParser.Parse(source, ref pos);
            ParserImpl.SkipSpaces(source, ref pos);
            if (pos == source.Length) return left;
            if (source[pos] == '^')
            {
                pos++;
                ParserImpl.SkipSpaces(source, ref pos);
                if (!TermParser.IsApplicable(source, pos))
                {
                    throw new ParserException("Invalid term at " + pos);
                }
                IExpression right = PowerParser.Parse(source, ref pos);
                return new PowerOperation(left, right);
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
            return source[pos] == '(' || NumberParser.IsApplicable(source, pos) || VariableParser.IsApplicable(source, pos) || source[pos] == 's' || source[pos] == 'c' || source[pos] == 'p' || source[pos] == 'e';
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
            if (pos + 1 < source.Length && source[pos] == 'p' && source[pos + 1] == 'i')
            {
                pos += 2;
                return new NumberExpr(3.141);
            }
            if (source[pos] == 'e')
            {
                pos++;
                return new NumberExpr(2.718);
            }
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
            if (pos < source.Length - 3 && source[pos] == 's' && source[pos + 1] == 'i' && source[pos + 2] == 'n' && source[pos + 3] == '(')
            {
                pos += 4;
                ParserImpl.SkipSpaces(source, ref pos);
                if (!ExprParser.IsApplicable(source, pos))
                {
                    throw new ParserException("Invalid expression at " + pos);
                }

                IExpression expr = ExprParser.Parse(source, ref pos);

                ParserImpl.SkipSpaces(source, ref pos);
                if (source[pos] != ')')
                {
                    throw new ParserException("Invalid expression at " + pos);
                }
                pos++;
                return new SinFunction(expr);
            }
            if (pos < source.Length - 3 && source[pos] == 'c' && source[pos + 1] == 'o' && source[pos + 2] == 's' && source[pos + 3] == '(')
            {
                pos += 4;
                ParserImpl.SkipSpaces(source, ref pos);
                if (!ExprParser.IsApplicable(source, pos))
                {
                    throw new ParserException("Invalid expression at " + pos);
                }

                IExpression expr = ExprParser.Parse(source, ref pos);

                ParserImpl.SkipSpaces(source, ref pos);
                if (source[pos] != ')')
                {
                    throw new ParserException("Invalid expression at " + pos);
                }
                pos++;
                return new CosFunction(expr);
            }
            if (NumberParser.IsApplicable(source, pos))
            {
                return NumberParser.Parse(source, ref pos);
            }


            return VariableParser.Parse(source, ref pos);
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
    class VariableParser
    {
        public static bool IsApplicable(string source, int pos)
        {
            if (pos >= source.Length)
            {
                return false;
            }
            return source[pos] == 'x';
        }
        public static VariableExpr Parse(string source, ref int pos)
        {
            if (IsApplicable(source, pos))
            {
                pos++;
                return new VariableExpr();
            }
            throw new ParserException("Expected variable at " + pos);
        }
    }
}
