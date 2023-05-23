using System.Diagnostics.CodeAnalysis;
using Rillium.Script.Exceptions;

namespace Rillium.Script
{
    internal static class ExceptionHelpers
    {
        [DoesNotReturn]
        public static void ThrowScriptException<T>(this Token token, string message)
            where T : Exception
        {
            var m = MessageWithLineNumber(message, token.Line);

            if (typeof(T) == typeof(BadNameException))
            {
                throw new BadNameException(m);
            }

            throw new ScriptException(m);
        }

        private static string MessageWithLineNumber(string message, int lineNumber) =>
            $"Line {lineNumber + 1}. {message}";
    }
}
