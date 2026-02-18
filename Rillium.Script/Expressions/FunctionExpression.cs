namespace Rillium.Script.Expressions
{
    internal class FunctionExpression : Expression
    {
        public string Name { get; }
        public IList<Expression> Arguments { get; }

        public FunctionExpression(Token token, string name, IList<Expression> arguments)
            : base(token)
        {
            name.ShouldNotBeNull();
            this.Name = name;
            this.Arguments = arguments;
        }

        public override Expression Evaluate(Scope scope)
        {
            var f = scope.GetFunction(this.Name, this.Arguments.Count);

            var functionArguments = new List<object>();
            for (var i = 0; i < this.Arguments.Count; i++)
            {
                var argumentType = f.ArgumentTokens[i];
                functionArguments.Add(this.Arguments[i].EvaluateToType(argumentType, scope));
            }

            return this.InvokeSync(f, functionArguments);
        }

        public override async Task<Expression> EvaluateAsync(Scope scope)
        {
            var f = scope.GetFunction(this.Name, this.Arguments.Count);

            var functionArguments = new List<object>();
            for (var i = 0; i < this.Arguments.Count; i++)
            {
                var argumentType = f.ArgumentTokens[i];
                var evaluated = await this.Arguments[i].EvaluateAsync(scope);
                functionArguments.Add(evaluated.EvaluateToType(argumentType, scope));
            }

            var input = f.ArgumentTokens.Count == 1
                ? functionArguments.First()
                : (dynamic)functionArguments;

            if (f.IsAsync && f.AsyncFunction != null)
            {
                var result = await f.AsyncFunction.Invoke(input);
                if (f.Out == LiteralTypeId.Number)
                {
                    return new NumberExpression(this.Token, (double)result);
                }

                throw new NotImplementedException(
                    $"Evaluation of async function '{f.Name}' with return type of '{f.Out}' not implemented.");
            }

            return this.InvokeSync(f, functionArguments);
        }

        private Expression InvokeSync(FunctionInfo f, List<object> functionArguments)
        {
            if (f.Out == LiteralTypeId.Number)
            {
                return new NumberExpression(this.Token, f.Function?.Invoke(
                       f.ArgumentTokens.Count == 1 ?
                       functionArguments.First() :
                       functionArguments));
            }

            throw new NotImplementedException($"Evaluation of function '{f.Name}' with return type of '{f.Out}' not implemented.");
        }
    }
}
