namespace Rillium.Script
{
    public class ScriptOptions
    {
        internal List<FunctionInfo> CustomFunctions { get; } = new List<FunctionInfo>();

        public ScriptOptions AddFunction(
            string name,
            Func<dynamic, dynamic> function,
            int argumentCount,
            LiteralTypeId returnType = LiteralTypeId.Number)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                Function = function,
                ArgumentTokens = Enumerable.Repeat(LiteralTypeId.Number, argumentCount).ToList(),
                Out = returnType,
            });

            return this;
        }

        public ScriptOptions AddAsyncFunction(
            string name,
            Func<dynamic, Task<dynamic>> asyncFunction,
            int argumentCount,
            LiteralTypeId returnType = LiteralTypeId.Number)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                IsAsync = true,
                AsyncFunction = asyncFunction,
                ArgumentTokens = Enumerable.Repeat(LiteralTypeId.Number, argumentCount).ToList(),
                Out = returnType,
            });

            return this;
        }
    }
}
