namespace Rillium.Script.Test
{
    public class FunctionInfo<T>
    {
        public string Name { get; set; }

        public int Arguments { get; set; }

        public Func<T> Function { get; set; }
    }
}
