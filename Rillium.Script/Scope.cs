namespace Rillium.Script
{
    public class Scope
    {
        private readonly IDictionary<string, object?> store = new Dictionary<string, object?>();

        public object? Get(string key) =>
            TryGet(key, out var value) ? value
            : throw new ArgumentException(
                string.Format(Constants.ExceptionMessages.NameDoesNotExist, key));

        public bool TryGet(string key, out object? value) =>
            store.TryGetValue(key, out value);

        public void Set(string key, object value) =>
            store[key] = value;
    }
}
