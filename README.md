
![Rillium Script 128](https://github.com/rilliumio/Rillium.Script/assets/126918909/630d8811-7647-4969-bc64-92204658bb53) 
# Rillium.Script
[![Rillium.Script](https://img.shields.io/nuget/v/Rillium.Script.svg?color=blue)](https://www.nuget.org/packages/Rillium.Script)
```cs
using Rillium.Script;

// Simple math parser
int a = Evaluator.Evaluate<int>("4 + 3 * 2");              // 10
int b = Evaluator.Evaluate<int>("var b = 4 + 3 * 2"; b;);  // 10

// Supports System.Math methods
double c = Evaluator.Evaluate<double>("Log(1.5)");         // 0.4054651081081644

// variables, loops, conditionals, and more.
string source = @"
       var d = 1;
       for(var i = 0; i < 10; i++)
       { d = d * 2; }
       return d;";

int d = Evaluator.Evaluate<int>(source);  // 1024
```

## Introduction
This is an implementation of a c-styled script parser and evaluator. It consists of a lexer, which converts raw source code into a sequence of tokens, and a parser, which turns this sequence of tokens into an abstract syntax tree (AST). The AST represents the program's structure and semantics, and is then used to generate executable code.

## Security 
Rillium.Script was created to provide basic scripting functionality that could be modified at runtime without the need to recompile or redeploy. When evaluating other options, such as Roslyn or other script engine projects, we found that:

* The scope of these projects was too large for our needs. They included features that we didn't need, such as debugging support or IDE integration.

* The risk of malicious code execution was too high, such as allowing users to define code sent to the `Roslyn.Compiler` directly.

We decided to create Rillium.Script because we wanted a simple, secure, and easy-to-use scripting engine. Rillium.Script is designed to be small in scope and to minimize the security attack surface. It is also easy to use, with a simple API that makes it easy to integrate into your applications.

## Performance

Rillium.Script implements the concept of an LR Parser, that reads the input string from left to right and produces a rightmost derivation in reverse. The parser builds a parse tree and analyzes the structure of the input string. One of the key advantages of the LR Parser is that it can handle a broad class of context-free grammar and generate a deterministic parser from the grammar rules.

The Rillium.Script parser is designed to be fast, reliable, and efficient. It's capable of handling a wide range of syntax structures, including conditional statements, loops, expressions, and more.
