
![Rillium Script 128](https://raw.githubusercontent.com/rilliumio/Rillium.Script/main/Rillium.Script.png) 
# Rillium.Script
[![Rillium.Script](https://img.shields.io/nuget/v/Rillium.Script.svg?color=blue)](https://www.nuget.org/packages/Rillium.Script)
[![Documentation](https://img.shields.io/badge/wiki-documentation-forestgreen)](https://github.com/rilliumio/Rillium.Script/wiki)
![Code Coverage](https://img.shields.io/badge/Code%20Coverage-96%25-forestgreen?style=flat)

Rillium.Script provides safe scripting functionality that can be modified at runtime without the need to recompile or re-deploy your application.

Rillium.Script is designed to resemble a simplified version of C#, making it familiar to users already comfortable with C-style languages.

```ts
using Rillium.Script;

// Simple math parser
int a = Evaluator.Evaluate<int>("4 + 3 * 2");              // 10
int b = Evaluator.Evaluate<int>("var b = 4 + 3 * 2; b;");  // 10

// Supports System.Math methods
double c = Evaluator.Evaluate<double>("Log(1.5)");         // 0.4054651081081644
```

Try it out: https://dotnetfiddle.net/KfzpMV

## Documentation
Refer to the documentation to get started with variables, loops, if-else statements, conditionals, and other essential features.

https://github.com/rilliumio/Rillium.Script/wiki

## Feedback Welcome!
If you discover bugs or deficiencies, please create a new [issue](https://github.com/rilliumio/Rillium.Script/issues) with an example script.

For new feature proposals, please raise them for [discussion](https://github.com/rilliumio/Rillium.Script/discussions).