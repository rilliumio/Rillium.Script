﻿namespace Rillium.Script
{
    public class AssignmentExpression : Expression
    {
        public VariableExpression Target { get; }
        public Expression Value { get; }

        public AssignmentExpression(VariableExpression target, Expression value)
        {
            Target = target;
            Value = value;
        }

        public override Expression Evaluate(Scope scope) => this;


        public void Set(Scope scope)
        {
            scope.Set(Target.Name.Value, Value.Evaluate(scope));
        }
    }
}
