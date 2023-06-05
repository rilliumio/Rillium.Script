using Rillium.Script.Exceptions;

namespace Rillium.Script
{
    internal class Scope
    {
        private readonly FunctionTable functions;
        private readonly IDictionary<string, object?> store = new Dictionary<string, object?>();

        public Scope(FunctionTable functions)
        {
            this.functions = functions;
        }

        public bool HasVariable(string name) => this.store.ContainsKey(name);

        public object? Get(Token token)
        {
            if (this.TryGet(token.Value, out var value)) { return value; }

            token.ThrowScriptException<BadNameException>(
                string.Format(Constants.ExceptionMessages.NameDoesNotExist, token.Value));

            return null;
        }

        public bool TryGet(string key, out object? value) =>
            this.store.TryGetValue(key, out value);

        public void Set(string key, object? value) =>
            this.store[key] = value;

        public FunctionInfo GetFunction(string name, int argumentCout) =>
            this.functions.GetFunction(name, argumentCout);
    }
}
