namespace Rillium.Script.Statements
{
    internal abstract class Statement
    {
        public abstract void Execute(Scope scope);

        public virtual Task ExecuteAsync(Scope scope)
        {
            this.Execute(scope);
            return Task.CompletedTask;
        }
    }
}
