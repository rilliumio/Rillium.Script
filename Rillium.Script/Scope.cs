namespace Rillium.Script
{
    public class Scope
    {
        private IDictionary<string, object> store = new Dictionary<string, object>();

        public object Get(string key)
        {
            return store[key];
        }

        public bool TryGet(string key, out object value) =>
            store.TryGetValue(key, out value);

        public void Set(string key, object value)
        {
            store[key] = value;
        }
    }
}
