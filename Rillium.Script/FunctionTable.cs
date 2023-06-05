using Rillium.Script.Exceptions;

namespace Rillium.Script
{
    internal class FunctionTable
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
            functionInfo.ArgumentTokens ??= new List<LiteralTypeId>();

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
                throw new BadNameException($"Unknown function name '{name}'.");
            }

            var overloads = functions[name];
            if (!overloads.ContainsKey(argumentCount))
            {
                throw new ScriptException(
                    $"No overload of function '{name}' that takes {argumentCount} arguments.");
            }

            return overloads[argumentCount];
        }
    }
}
