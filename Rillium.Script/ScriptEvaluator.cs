using System.Globalization;

namespace Rillium.Script
{
    public static class Evaluator
    {
        public static (object output, string console) Run(string script)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            var output = Run(script, streamWriter);

            memoryStream.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(memoryStream);

            return (output, streamReader.ReadToEnd());
        }

        public static object Run(string script, StreamWriter output)
        {
            var functionTable = new FunctionTable();
            var lexer = new Lexer(script, functionTable.GetFunctionNames());
            var parser = new Parser(lexer, output, functionTable);

            return parser.Parse();
        }

        /// <summary>
        ///     Evaluate a script that returns 
        ///     a value.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="source">The source script.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T? Evaluate<T>(string source)
        {
            var (result, console) = Run(source);
            if (result is T) { return (T)result; }
            var typeT = typeof(T);

            if (result == null && default(T) == null) { return default; }
            if (result != null && result.GetType() == typeT) { return (T)(object)result; }


            if (result is NumberExpression numberExpression)
            {
                var m = numberExpression.Value;
                if (m is double d)
                {
                    return (T)Convert.ChangeType(d, typeof(T));
                }
            }

            if (result is LiteralExpression literalExpression)
            {
                var m = literalExpression.Value.Value;
                if (m is double lvd)
                {
                    return (T)Convert.ChangeType(lvd, typeof(T));
                }
            }

            if (result is double doubleResult)
            {
                return (T)Convert.ChangeType(doubleResult, typeof(T));
            }

            if (result is string s)
            {
                if (typeT == typeof(double))
                {
                    var d = double.Parse(s, CultureInfo.InvariantCulture);
                    return (T)(object)d;
                }

                if (typeT == typeof(int))
                {
                    var d = int.Parse(s, CultureInfo.InvariantCulture);
                    return (T)(object)d;
                }

                if (typeT == typeof(float))
                {
                    var d = float.Parse(s, CultureInfo.InvariantCulture);
                    return (T)(object)d;
                }

                if (typeT == typeof(long))
                {
                    var d = long.Parse(s, CultureInfo.InvariantCulture);
                    return (T)(object)d;
                }
            }

            if (result is List<double>)
            {
                return (T)Convert.ChangeType(result, typeof(double));
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
