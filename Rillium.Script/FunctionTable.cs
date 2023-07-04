using Rillium.Script.Exceptions;

namespace Rillium.Script
{
    internal class FunctionTable
    {
        private readonly IDictionary<string, IDictionary<int, FunctionInfo>> functions;

        public FunctionTable()
        {
            this.functions = new Dictionary<string, IDictionary<int, FunctionInfo>>();

            var defaultFunctions = FunctionHelpers.DefaultFunctions();
            foreach (var function in defaultFunctions)
            {
                this.AddFunction(function);
            }
        }

        public ISet<string> GetFunctionNames() => this.functions.Keys.ToHashSet();

        public void AddFunction(FunctionInfo functionInfo)
        {
            functionInfo.ArgumentTokens ??= new List<LiteralTypeId>();

            if (!this.functions.ContainsKey(functionInfo.Name))
            {
                this.functions[functionInfo.Name] = new Dictionary<int, FunctionInfo>();
            }

            this.functions[functionInfo.Name][functionInfo.ArgumentTokens.Count] = functionInfo;
        }

        public FunctionInfo GetFunction(string name, int argumentCount)
        {
            if (!this.functions.ContainsKey(name))
            {
                throw new BadNameException($"Unknown function name '{name}'.");
            }

            var overloads = this.functions[name];
            if (!overloads.ContainsKey(argumentCount))
            {
                throw new ScriptException(
                    $"No overload of function '{name}' that takes {argumentCount} arguments.");
            }

            return overloads[argumentCount];
        }
    }
}
