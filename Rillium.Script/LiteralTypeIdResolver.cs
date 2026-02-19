namespace Rillium.Script
{
    internal static class LiteralTypeIdResolver
    {
        public static LiteralTypeId Resolve<T>()
        {
            Type t = typeof(T);
            if (t == typeof(double) || t == typeof(float) || t == typeof(int) ||
                t == typeof(long)   || t == typeof(short)  || t == typeof(byte) ||
                t == typeof(decimal))
                return LiteralTypeId.Number;
            if (t == typeof(string)) return LiteralTypeId.String;
            if (t == typeof(bool))   return LiteralTypeId.Bool;
            throw new ArgumentException(
                $"Unsupported type '{t.FullName}' for script function registration. " +
                $"Supported types: numeric types, string, bool.");
        }
    }
}
