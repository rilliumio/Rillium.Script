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

        public string EvaluateStatement()
        {
            using (var m = new MemoryStream())
            {
                using (var w = new StreamWriter(m))
                {
                    _parser.ParseStatements(w);
                    w.Flush();


                    // Reset the stream position to the beginning
                    m.Seek(0, SeekOrigin.Begin);

                    using (var sr = new StreamReader(m))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        public void Evaluate(StreamWriter w)
        {
            _parser.ParseStatements(w);
        }
    }
}
