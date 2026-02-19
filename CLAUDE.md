# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Test Commands

```sh
dotnet build --configuration Release
dotnet test
dotnet test --filter "FullyQualifiedName~Rillium.Script.Test.ArrayTests"  # run a single test class
dotnet test --filter "FullyQualifiedName~Rillium.Script.Test.ArrayTests.ArrayLength"  # run a single test method
```

No separate lint command — style is enforced via `.editorconfig` in `Rillium.Script/`.

## Project Structure

- **Rillium.Script/** — Main library (NuGet package), multi-targets `net6.0` and `net8.0`
- **Rillium.Script.Test/** — MSTest unit tests (has `InternalsVisibleTo` access)

## Architecture

Rillium.Script is a runtime scripting engine with C#-like syntax. It follows a classic **Lexer → Parser → AST → Evaluator** pipeline:

1. **`Evaluator`** (static, only public API) — Entry point. `Evaluate<T>()` returns a typed result; `Run()` returns output + console text. Passes `args` parameter array into scope as the `args` variable.

2. **`Lexer`** (internal) — Tokenizes source into `Token` objects. Tracks line numbers for error reporting. Supports `//` and `/* */` comments.

3. **`Parser`** (internal) — Recursive descent parser. Consumes tokens, builds and immediately evaluates AST nodes. Uses `Scope` for variable storage and `FunctionTable` for callable functions.

4. **`Scope`** (internal) — Flat `Dictionary<string, object?>` variable store. The special key `__return__` holds the implicit return value (last expression result).

5. **`FunctionTable`** (internal) — Registry of built-in functions. Pre-populated with all `System.Math` methods via `FunctionHelpers.DefaultFunctions()`. Supports overloads keyed by argument count.

### AST Nodes

- **Expressions** (`Expressions/`) — All extend `Expression` with `Evaluate(Scope)`. Types: `NumberExpression` (double), `LiteralExpression` (string/bool/null), `BinaryExpression`, `AssignmentExpression`, `VariableExpression`, `IdentifierExpression`, `IndexExpression`, `ArrayExpression`, `ArraySummaryExpression` (.Length/.Sum()/.Min() etc.), `FunctionExpression`, `TernaryExpression`.

- **Statements** (`Statements/`) — All extend `Statement` with `Execute(Scope)`. Types: `BlockStatement`, `DeclarationStatement` (var), `ExpressionStatement`, `ForLoopStatement`, `IfStatement`, `ReturnStatement`.

### Control Flow

`ReturnStatement` throws `ReturnStatementException` (internal) which is caught by the parser to implement early return from blocks/loops.

### Exceptions (public)

- `ScriptException` — base for all script errors
- `SyntaxException : ScriptException` — parse/tokenization errors
- `BadNameException : ScriptException` — undefined variable/function

### Type System

Numbers are `double` internally. `Evaluate<T>` handles conversion to `int`, `long`, `decimal`, etc. and array types.

## Code Style

- Use `this.` qualifier for fields, methods, events, properties
- Explicit types preferred over `var`
- Block-scoped namespaces
- Allman brace style (braces on new lines)
- 4-space indentation

## CI/CD

GitHub Actions (`.github/workflows/starter.yml`): build → test with code coverage (minimum 95% threshold) → publish to NuGet on `release/**` branches.
