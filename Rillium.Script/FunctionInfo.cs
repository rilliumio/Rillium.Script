namespace Rillium.Script
{
    public class FunctionInfo
    {
        public string Name { get; set; }
        public IList<LiteralTypeId> ArgumentTokens { get; set; }

        public LiteralTypeId Out { get; set; }

        public Func<dynamic, dynamic> Function { get; set; }
    }
}
