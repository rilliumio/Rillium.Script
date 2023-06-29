
![Rillium Script 128](https://github.com/rilliumio/Rillium.Script/assets/126918909/630d8811-7647-4969-bc64-92204658bb53) 
# Rillium.Script

```cs
using Rillium.Script;

// Simple math parser
int a = Evaluator.Evaluate<int>("(4 + 3) * 2 - 3"); // 11

// Supports System.Math methods
double b = Evaluator.Evaluate<double>("Log(1.5)"); // 0.4054651081081644

// variables, loops, conditionals, and more.
string source = @"
 var j = 1;
 for(var i = 0; i < 10; i++)
 { j = j * 2; }
 return j;";

int c = Evaluator.Evaluate<int>(source); // 1024
```

## Introduction
This is an implementation of a c-styled script parser and evaluator. It consists of a lexer, which converts raw source code into a sequence of tokens, and a parser, which turns this sequence of tokens into an abstract syntax tree (AST). The AST represents the program's structure and semantics, and is then used to generate executable code.

## LR Parser

The LR Parser implements a bottom-up parser that reads the input string from left to right and produces a rightmost derivation in reverse. The parser builds a parse tree and analyzes the structure of the input string. One of the key advantages of the LR Parser is that it can handle a broad class of context-free grammar and generate a deterministic parser from the grammar rules.

The Rillium.Script LR parser is designed to be fast, reliable, and efficient. It's capable of handling a wide range of syntax structures, including conditional statements, loops, expressions, and more.
