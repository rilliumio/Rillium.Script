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
            var lexer = new Lexer(script);
            var parser = new SyntaxParser(lexer, output);

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
        public static T Evaluate<T>(string source) where T : IConvertible
        {
            var (result, console) = Run(source);
            if (result is T) { return (T)result; }
            var typeT = typeof(T);

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

            throw new NotImplementedException($"Could not convert output type '{result.GetType().Name}' to type '{typeT.Name}'");
        }
    }
}
