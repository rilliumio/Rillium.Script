namespace Rillium.Script
{
    public class ScriptOptions
    {
        internal List<FunctionInfo> CustomFunctions { get; } = new List<FunctionInfo>();

        // ── AddFunction (sync, returns value) ───────────────────────────────

        public ScriptOptions AddFunction<TOut>(string name, Func<TOut> function)
        {
            LiteralTypeId returnType = LiteralTypeIdResolver.Resolve<TOut>();
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                Function = (_) => (dynamic)function()!,
                ArgumentTokens = new List<LiteralTypeId>(),
                Out = returnType,
            });
            return this;
        }

        public ScriptOptions AddFunction<T1, TOut>(string name, Func<T1, TOut> function)
        {
            LiteralTypeId returnType = LiteralTypeIdResolver.Resolve<TOut>();
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                Function = (args) => (dynamic)function((T1)args)!,
                ArgumentTokens = new List<LiteralTypeId> { LiteralTypeIdResolver.Resolve<T1>() },
                Out = returnType,
            });
            return this;
        }

        public ScriptOptions AddFunction<T1, T2, TOut>(string name, Func<T1, T2, TOut> function)
        {
            LiteralTypeId returnType = LiteralTypeIdResolver.Resolve<TOut>();
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                Function = (args) => (dynamic)function((T1)args[0], (T2)args[1])!,
                ArgumentTokens = new List<LiteralTypeId>
                {
                    LiteralTypeIdResolver.Resolve<T1>(),
                    LiteralTypeIdResolver.Resolve<T2>(),
                },
                Out = returnType,
            });
            return this;
        }

        // ── AddFunctionAsync (async, returns Task<TOut>) ─────────────────────

        public ScriptOptions AddFunctionAsync<TOut>(string name, Func<Task<TOut>> function)
        {
            LiteralTypeId returnType = LiteralTypeIdResolver.Resolve<TOut>();
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                IsAsync = true,
                AsyncFunction = async (_) => (dynamic)(await function())!,
                ArgumentTokens = new List<LiteralTypeId>(),
                Out = returnType,
            });
            return this;
        }

        public ScriptOptions AddFunctionAsync<T1, TOut>(string name, Func<T1, Task<TOut>> function)
        {
            LiteralTypeId returnType = LiteralTypeIdResolver.Resolve<TOut>();
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                IsAsync = true,
                AsyncFunction = async (args) => (dynamic)(await function((T1)args))!,
                ArgumentTokens = new List<LiteralTypeId> { LiteralTypeIdResolver.Resolve<T1>() },
                Out = returnType,
            });
            return this;
        }

        public ScriptOptions AddFunctionAsync<T1, T2, TOut>(string name, Func<T1, T2, Task<TOut>> function)
        {
            LiteralTypeId returnType = LiteralTypeIdResolver.Resolve<TOut>();
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                IsAsync = true,
                AsyncFunction = async (args) => (dynamic)(await function((T1)args[0], (T2)args[1]))!,
                ArgumentTokens = new List<LiteralTypeId>
                {
                    LiteralTypeIdResolver.Resolve<T1>(),
                    LiteralTypeIdResolver.Resolve<T2>(),
                },
                Out = returnType,
            });
            return this;
        }

        // ── AddAction (sync, void) ───────────────────────────────────────────

        public ScriptOptions AddAction(string name, Action action)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                Function = (_) => { action(); return (dynamic)null!; },
                ArgumentTokens = new List<LiteralTypeId>(),
                Out = LiteralTypeId.Null,
            });
            return this;
        }

        public ScriptOptions AddAction<T1>(string name, Action<T1> action)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                Function = (args) => { action((T1)args); return (dynamic)null!; },
                ArgumentTokens = new List<LiteralTypeId> { LiteralTypeIdResolver.Resolve<T1>() },
                Out = LiteralTypeId.Null,
            });
            return this;
        }

        public ScriptOptions AddAction<T1, T2>(string name, Action<T1, T2> action)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                Function = (args) => { action((T1)args[0], (T2)args[1]); return (dynamic)null!; },
                ArgumentTokens = new List<LiteralTypeId>
                {
                    LiteralTypeIdResolver.Resolve<T1>(),
                    LiteralTypeIdResolver.Resolve<T2>(),
                },
                Out = LiteralTypeId.Null,
            });
            return this;
        }

        // ── AddActionAsync (async, void) ─────────────────────────────────────

        public ScriptOptions AddActionAsync(string name, Func<Task> action)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                IsAsync = true,
                AsyncFunction = async (_) => { await action(); return (dynamic)null!; },
                ArgumentTokens = new List<LiteralTypeId>(),
                Out = LiteralTypeId.Null,
            });
            return this;
        }

        public ScriptOptions AddActionAsync<T1>(string name, Func<T1, Task> action)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                IsAsync = true,
                AsyncFunction = async (args) => { await action((T1)args); return (dynamic)null!; },
                ArgumentTokens = new List<LiteralTypeId> { LiteralTypeIdResolver.Resolve<T1>() },
                Out = LiteralTypeId.Null,
            });
            return this;
        }

        public ScriptOptions AddActionAsync<T1, T2>(string name, Func<T1, T2, Task> action)
        {
            this.CustomFunctions.Add(new FunctionInfo
            {
                Name = name,
                IsAsync = true,
                AsyncFunction = async (args) => { await action((T1)args[0], (T2)args[1]); return (dynamic)null!; },
                ArgumentTokens = new List<LiteralTypeId>
                {
                    LiteralTypeIdResolver.Resolve<T1>(),
                    LiteralTypeIdResolver.Resolve<T2>(),
                },
                Out = LiteralTypeId.Null,
            });
            return this;
        }
    }
}
