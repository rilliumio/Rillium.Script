namespace Rillium.Script.Statements
{
    internal abstract class Statement
    {
        public abstract void Execute(Scope scope);
    }
}
