namespace Rillium.Script
{
    public class ScriptEvaluator
    {
        private readonly string _script;
        private readonly Lexer _lexer;
        private readonly SyntaxParser _parser;

        public ScriptEvaluator(string script)
        {
            _script = script;
            _lexer = new Lexer(_script);
            _parser = new SyntaxParser(_lexer);
        }

        public double EvaluateExpression()
        {
            var expression = (LiteralExpression)_parser.ParseExpression();
            if (expression.Value.TypeId == LiteralTypeId.Number && expression.Value.Value is double d) { return d; }
            return double.Parse(expression.Value?.ToString() ?? "NaN");
        }

        public void EvaluateStatement()
        {
            var s = _parser.ParseStatements();
        }
    }
}
