using Rillium.Script.Exceptions;

namespace Rillium.Script.Expressions
{
    internal class AssignmentExpression : Expression
    {
        public VariableExpression Target { get; }
        public Expression Value { get; }

        public AssignmentExpression(
            Token token,
            VariableExpression target, Expression value)
            : base(token)
        {
            Target = target;
            Value = value;
        }

        public override Expression Evaluate(Scope scope) => this;


        public void Set(Scope scope)
        {
            if (!scope.HasVariable(Target.Name.Value))
            {
                Token.ThrowScriptException<BadNameException>(
                    string.Format(Constants.ExceptionMessages.NameDoesNotExist, Target.Name.Value));
            }

            scope.Set(Target.Name.Value, Value.Evaluate(scope));
        }
    }
}
