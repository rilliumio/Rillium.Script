using Rillium.Script.Exceptions;

namespace Rillium.Script
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;
        private int _lineNumber;
        private readonly ISet<string> functionNames;

        public Lexer(string input, ISet<string> functionNames)
        {
            this._input = input;
            this._position = 0;
            this.functionNames = functionNames;
        }

        public Token NextToken()
        {
            while (this._position < this._input.Length)
            {
                var currentChar = this._input[this._position];

                // Whitespace
                if (char.IsWhiteSpace(currentChar))
                {
                    if (currentChar == '\n') { this._lineNumber++; }
                    this.ConsumeChar();
                    continue;
                }

                // Operators and delimiters
                if (currentChar == '+')
                {
                    this.ConsumeChar();
                    var nextChar = (this._input[this._position]);
                    if (nextChar == '+')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.PlusPlus, "++", this._lineNumber);
                    }

                    return new Token(TokenId.Plus, "+", this._lineNumber);
                }
                if (currentChar == '-')
                {
                    this.ConsumeChar();
                    var nextChar = (this._input[this._position]);
                    if (nextChar == '-')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.MinusMinus, "--", this._lineNumber);
                    }

                    return new Token(TokenId.Minus, "--", this._lineNumber);
                }
                if (currentChar == '*')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Star, "*", this._lineNumber);
                }
                if (currentChar == '/')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Slash, "/", this._lineNumber);
                }
                if (currentChar == '(')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.LeftParen, "(", this._lineNumber);
                }
                if (currentChar == ')')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.RightParen, ")", this._lineNumber);
                }
                if (currentChar == '{')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.LeftBrace, "{", this._lineNumber);
                }
                if (currentChar == '}')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.RightBrace, "}", this._lineNumber);
                }

                if (currentChar == '[')
                {
                    this.ConsumeChar();
                    if (this._input[this._position] == ']')
                    {
                        throw new SyntaxException($"Line {this._lineNumber + 1}. Value expected.");
                    }

                    return new Token(TokenId.LeftSquareBracket, "[", this._lineNumber);
                }

                if (currentChar == ']')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.RightSquareBracket, "]", this._lineNumber);
                }

                if (currentChar == ',')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Comma, ",", this._lineNumber);
                }

                if (currentChar == ';')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Semicolon, ";", this._lineNumber);
                }

                if (currentChar == '=')
                {
                    this.ConsumeChar();
                    if (this._input[this._position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.EqualEqual, "==", this._lineNumber);
                    }

                    return new Token(TokenId.Equal, "=", this._lineNumber);
                }

                if (currentChar == '>')
                {
                    this.ConsumeChar();
                    if (this._input[this._position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.GreaterEqual, ">=", this._lineNumber);
                    }

                    return new Token(TokenId.Greater, ">", this._lineNumber);
                }

                if (currentChar == '<')
                {
                    this.ConsumeChar();
                    if (this._input[this._position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.LessEqual, "<=", this._lineNumber);
                    }

                    return new Token(TokenId.Less, "<", this._lineNumber);
                }

                if (currentChar == '!')
                {
                    this.ConsumeChar();
                    if (this._input[this._position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.BangEqual, "!=", this._lineNumber);
                    }

                    return new Token(TokenId.Bang, "!", this._lineNumber);
                }


                if (currentChar == '.')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Dot, ".", this._lineNumber);
                }

                if (currentChar == '%')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Percent, "%", this._lineNumber);
                }


                // Numbers
                if (char.IsDigit(currentChar))
                {
                    return new Token(TokenId.Number, this.ConsumeNumber(), this._lineNumber);
                }

                if (currentChar == '"' || currentChar == '\'')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.String, this.ConsumeString(currentChar), this._lineNumber);
                }

                // Identifiers
                if (char.IsLetter(currentChar))
                {
                    var word = this.ConsumeIdentifier();
                    if (word == "if") { return new Token(TokenId.If, null, this._lineNumber); }
                    if (word == "else") { return new Token(TokenId.Else, null, this._lineNumber); }
                    if (word == "for") { return new Token(TokenId.For, null, this._lineNumber); }
                    if (word == "var") { return new Token(TokenId.Var, null, this._lineNumber); }
                    if (word == "return") { return new Token(TokenId.Return, null, this._lineNumber); }
                    if (word == "true") { return new Token(TokenId.True, null, this._lineNumber); }
                    if (word == "false") { return new Token(TokenId.False, null, this._lineNumber); }

                    if (this.functionNames.Contains(word))
                    {
                        return new Token(TokenId.Function, word, this._lineNumber);
                    }

                    return new Token(TokenId.Identifier, word, this._lineNumber);
                }

                // Invalid character
                throw new SyntaxException($"Line {this._lineNumber + 1}. Invalid character '{currentChar}' at position {this._position}");
            }

            return new Token(TokenId.Eof, string.Empty, this._lineNumber);
        }

        private void ConsumeChar()
        {
            this._position++;
        }

        private string ConsumeNumber()
        {
            var start = this._position;

            while (this._position < this._input.Length && (char.IsDigit(this._input[this._position]) || this._input[this._position] == '.'))
            {
                this._position++;
            }

            return this._input.Substring(start, this._position - start);
        }

        private string ConsumeIdentifier()
        {
            var start = this._position;
            while (this._position < this._input.Length && (char.IsLetterOrDigit(this._input[this._position]) || this._input[this._position] == '_'))
            {
                this._position++;
            }
            return this._input.Substring(start, this._position - start);
        }

        private string ConsumeString(char quote)
        {
            var start = this._position;
            while (this._position < this._input.Length && this._input[this._position] != quote)
            {
                this._position++;
            }

            return (this._position < this._input.Length && this._input[this._position] == quote)
                ? this._input[start..this._position++]
                : throw new ScriptException($"Line {this._lineNumber + 1}. {quote} expected.");
        }
    }
}
