# Rillium.Script

## Introduction

This is an implementation of a compiler for a programming language. It consists of a lexer, which converts raw source code into a sequence of tokens, and a parser, which turns this sequence of tokens into an abstract syntax tree (AST). The AST represents the program's structure and semantics, and is then used to generate executable code. The compiler uses the visitor pattern to traverse the AST and generate the final output.

## LR Parser

The LR Parser implements a bottom-up parser that reads the input string from left to right and produces a rightmost derivation in reverse. The parser builds a parse tree and analyzes the structure of the input string. One of the key advantages of the LR Parser is that it can handle a broad class of context-free grammars and generate a deterministic parser from the grammar rules.

The Rillium.Script LR parser is designed to be provide fast, reliable, and efficient. It's capable of handling a wide range of syntax structures, including conditional statements, loops, expressions, and more.

So why go with an LR Parser? Well, its a trade off. The LR parser is more complicated to implement, but offers superior parsing and error handling capabilities, but most choosen for high performance. It's also flexible, which means it can be adapted to various languages requirements and custom syntax rules.
