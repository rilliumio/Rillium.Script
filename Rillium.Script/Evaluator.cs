namespace Rillium.Script
{
    public static class Evaluator
    {
        public static CompiledScript Compile(string source)
        {
            var functionTable = new FunctionTable();
            var lexer = new Lexer(source, functionTable.GetFunctionNames());
            var parser = new Parser(lexer, StreamWriter.Null, functionTable);

            var statements = parser.BuildStatements();
            return new CompiledScript(statements, functionTable);
        }

        public static (object? output, string console) Run(string script, params object[]? args)
            => Compile(script).Run(args);

        public static object? Run(string script, StreamWriter output, params object[]? args)
            => Compile(script).Run(output, args);

        /// <summary>
        ///     Evaluate a script that returns
        ///     a value.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="source">The source script.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T? Evaluate<T>(string source, params object[]? args)
            => Compile(source).Evaluate<T>(args);
    }
}
