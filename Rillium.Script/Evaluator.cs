using Rillium.Script.Expressions;

namespace Rillium.Script
{
    public static class Evaluator
    {
        public static (object? output, string console) Run(string script, params object[]? args)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            var output = Run(script, streamWriter, args);

            memoryStream.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(memoryStream);

            return (output, streamReader.ReadToEnd());
        }

        public static object? Run(string script, StreamWriter output, params object[]? args)
        {
            var functionTable = new FunctionTable();
            var lexer = new Lexer(script, functionTable.GetFunctionNames());
            var parser = new Parser(lexer, output, functionTable);

            return parser.Parse(args);
        }

        /// <summary>
        ///     Evaluate a script that returns 
        ///     a value.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="source">The source script.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T? Evaluate<T>(string source, params object[]? args)
        {
            var (result, console) = Run(source, args);
            if (result is T t) { return t; }
            var typeT = typeof(T);

            if (result is NumberExpression numberExpression && numberExpression.Value is double nev)
            {
                return (T)Convert.ChangeType(nev, typeof(T));
            }

            if (result is LiteralExpression literalExpression)
            {
                return (T?)Convert.ChangeType(literalExpression.Value.Value, typeof(T));
            }

            if (result is double doubleResult)
            {
                return (T)Convert.ChangeType(doubleResult, typeof(T));
            }

            if (result is List<object> olist)
            {
                if (typeT == typeof(byte[]))
                {
                    return (T)(object)(olist.Select(x =>
                          x is double d ? (byte)d
                        : x is short s ? (byte)s
                        : x is int i ? (byte)i
                        : x is byte b ? b
                        : throw new ArgumentException(x.GetType().Name)).ToArray());
                }

                if (typeT == typeof(short[]))
                {
                    return (T)(object)(olist.Select(x =>
                          x is double d ? (short)d
                        : x is short s ? s
                        : x is int i ? (short)i
                        : x is byte b ? (short)b
                        : throw new ArgumentException(x.GetType().Name)).ToArray());
                }

                if (typeT == typeof(int[]))
                {
                    return (T)(object)(olist.Select(x =>
                          x is int i ? i
                        : x is double d ? (int)d
                        : throw new ArgumentException(x.GetType().Name)).ToArray());
                }

                if (typeT == typeof(double[]))
                {
                    return (T)(object)(olist.Select(x =>
                          x is double d ? d
                        : x is int i ? (double)i
                        : throw new ArgumentException(x.GetType().Name)).ToArray());
                }
            }


            return (result == null)
                ? throw new ArgumentException($"Could not convert null output to type '{typeT.Name}'")
                : throw new ArgumentException($"Could not convert output type '{result.GetType().Name}' to type '{typeT.Name}'");
        }
    }
}
