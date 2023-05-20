﻿namespace Rillium.Script
{
    public class FunctionExpression : Expression
    {
        public string Name { get; }
        public IList<Expression> Arguments { get; }

        public FunctionExpression(string name, IList<Expression> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public override Expression Evaluate(Scope scope)
        {
            var f = scope.GetFunction(Name, Arguments.Count);

            var functionArguments = new List<object>();
            for (var i = 0; i < Arguments.Count; i++)
            {
                var argumentType = f.ArgumentTokens[i];
                functionArguments.Add(Arguments[i].EvaluateToType(argumentType, scope));
            }

            if (f.Out == LiteralTypeId.Number)
            {
                return new NumberExpression(f.Function?.Invoke(
                       (f.ArgumentTokens.Count == 1) ?
                       functionArguments.First() :
                       functionArguments));
            }

            throw new NotImplementedException($"Evaluation of function '{f.Name}' with return type of '{f.Out}' not implemented.");
        }
    }
}
