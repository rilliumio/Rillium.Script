using Rillium.Script.Exceptions;
using Rillium.Script.Expressions;
using Rillium.Script.Statements;

namespace Rillium.Script
{
    public class CompiledScript
    {
        private readonly List<Statement> statements;
        private readonly FunctionTable functions;

        internal CompiledScript(List<Statement> statements, FunctionTable functions)
        {
            this.statements = statements;
            this.functions = functions;
        }

        public T? Evaluate<T>(params object[]? args)
        {
            var (result, console) = this.Run(args);
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

        public (object? output, string console) Run(params object[]? args)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            var output = this.Run(streamWriter, args);

            memoryStream.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(memoryStream);

            return (output, streamReader.ReadToEnd());
        }

        public object? Run(StreamWriter output, params object[]? args)
        {
            var scope = new Scope(this.functions);
            scope.InitializeScopeArguments(args);

            return Execute(this.statements, scope, output);
        }

        internal static object? Execute(List<Statement> statements, Scope scope, StreamWriter output)
        {
            foreach (var statement in statements)
            {
                if (statement is ReturnStatement returnStatement)
                {
                    return returnStatement.EvaluateReturnExpression(scope);
                }

                try
                {
                    statement.Execute(scope);
                }
                catch (ReturnStatementException returnStatementException)
                {
                    return returnStatementException.returnStatement.EvaluateReturnExpression(scope);
                }
            }

            output.Flush();

            if (scope.TryGet(Constants.OutputValueKey, out var outputValue))
            {
                if (outputValue is Expression ex) { return ex.Evaluate(scope); }
                return outputValue;
            }

            return null;
        }
    }
}
