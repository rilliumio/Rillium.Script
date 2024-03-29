﻿namespace Rillium.Script.Expressions
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

