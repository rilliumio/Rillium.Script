namespace Rillium.Script
{
    public class IdentifierExpression : Expression
    {
        public string Name { get; }

        public IdentifierExpression(string name)
        {
            Name = name;
        }

        public override Expression Evaluate(Scope scope)
        {
            if (scope.TryGet(Name, out object o) && o != null)
            {
                if (o is NumberExpression numberExpression) { return numberExpression; }

                throw new ArgumentException($"could not evaluate '{Name}'.");
            };

            return this;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitIdentifierExpression(this);
        }
    }
}
