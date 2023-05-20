namespace Rillium.Script
{
    public class FunctionTable
    {
        private readonly IDictionary<string, IDictionary<int, FunctionInfo>> functions;

        public FunctionTable()
        {
            functions = new Dictionary<string, IDictionary<int, FunctionInfo>>();

            var defaltFunctions = FunctionHelpers.DefaultFunctions();
            foreach (var function in defaltFunctions)
            {
                AddFunction(function);
            }
        }

        public ISet<string> GetFunctionNames() => functions.Keys.ToHashSet();

        public void AddFunction(FunctionInfo functionInfo)
        {
            if (functionInfo.ArgumentTokens == null)
            {
                functionInfo.ArgumentTokens = new List<LiteralTypeId>();
            }

            if (!functions.ContainsKey(functionInfo.Name))
            {
                functions[functionInfo.Name] = new Dictionary<int, FunctionInfo>();
            }

            functions[functionInfo.Name][functionInfo.ArgumentTokens.Count] = functionInfo;
        }

        public FunctionInfo GetFunction(string name, int argumentCount)
        {
            if (!functions.ContainsKey(name))
            {
                throw new ArgumentException($"Unknown function name '{name}'.");
            }

            var overloads = functions[name];
            if (!overloads.ContainsKey(argumentCount))
            {
                throw new ArgumentException($"No overload of function '{name}' that takes {argumentCount} arguments.");
            }

            return overloads[argumentCount];
        }
    }
}
