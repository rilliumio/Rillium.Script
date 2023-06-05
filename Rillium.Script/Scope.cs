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

            throw new BadNameException(
                $"Line {token.Line + 1}. " +
                $"{string.Format(Constants.ExceptionMessages.NameDoesNotExist, token.Value)}");
        }

        public bool TryGet(string key, out object? value) =>
            this.store.TryGetValue(key, out value);

        public void Set(string key, object? value) =>
            this.store[key] = value;

        public FunctionInfo GetFunction(string name, int argumentCount) =>
            this.functions.GetFunction(name, argumentCount);
    }
}
