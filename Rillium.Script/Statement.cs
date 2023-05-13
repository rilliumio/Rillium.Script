﻿namespace Rillium.Script
{
    public abstract class Statement
    {
        public abstract T Accept<T>(IStatementVisitor<T> visitor);

        public abstract void Execute(Scope scope);
    }
}
