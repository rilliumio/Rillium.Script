using Rillium.Script.Statements;

namespace Rillium.Script.Exceptions
{
    internal class ReturnStatementException : ScriptException
    {
        public ReturnStatement returnStatement { get; set; }
        public ReturnStatementException(ReturnStatement returnStatement)
        {
            this.returnStatement = returnStatement;
        }
    }
}
