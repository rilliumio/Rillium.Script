﻿using Rillium.Script.Exceptions;

namespace Rillium.Script
{
    public class Scope
    {
        private readonly FunctionTable functions;
        private readonly IDictionary<string, object?> store = new Dictionary<string, object?>();

        public Scope(FunctionTable functions)
        {
            this.functions = functions;
        }

        public bool HasVariable(string name) => store.ContainsKey(name);

        public object? Get(Token token)
        {
            if (TryGet(token.Value, out var value)) { return value; }

            token.ThrowScriptException<BadNameException>(
                string.Format(Constants.ExceptionMessages.NameDoesNotExist, token.Value));

            return null;
        }

        public bool TryGet(string key, out object? value) =>
            store.TryGetValue(key, out value);

        public void Set(string key, object value) =>
            store[key] = value;

        public FunctionInfo GetFunction(string name, int arguementCout) =>
            functions.GetFunction(name, arguementCout);
    }
}
