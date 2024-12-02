using Rillium.Script.Exceptions;

namespace Rillium.Script
{
    /// <summary>
    ///  The lexer is responsible for breaking down the input script into meaningful tokens,
    ///  which are subsequently used by the <see cref="Parser"/> for further analysis and interpretation.
    /// </summary>
    internal class Lexer
    {
        private readonly string _input;
        private int position;
        private int lineNumber;
        private readonly ISet<string> functionNames;

        /// <summary>
        ///     Initialize.
        /// </summary>
        /// <param name="input">The input script.</param>
        /// <param name="functionNames">Function names.</param>
        public Lexer(string input, ISet<string> functionNames)
        {
            this._input = input;
            this.position = 0;
            this.functionNames = functionNames;
        }

        /// <summary>
        ///     Get the next token.
        /// </summary>
        /// <returns>The next token.</returns>
        public Token NextToken()
        {
            while (this.position < this._input.Length)
            {
                var currentChar = this._input[this.position];

                // Whitespace
                if (char.IsWhiteSpace(currentChar))
                {
                    if (currentChar == '\n') { this.lineNumber++; }
                    this.ConsumeChar();
                    continue;
                }

                // Operators and delimiters
                if (currentChar == '+')
                {
                    this.ConsumeChar();
                    var nextChar = (this._input[this.position]);
                    if (nextChar == '+')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.PlusPlus, "++", this.lineNumber);
                    }

                    return new Token(TokenId.Plus, "+", this.lineNumber);
                }
                if (currentChar == '-')
                {
                    this.ConsumeChar();
                    var nextChar = (this._input[this.position]);
                    if (nextChar == '-')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.MinusMinus, "--", this.lineNumber);
                    }

                    return new Token(TokenId.Minus, "--", this.lineNumber);
                }
                if (currentChar == '*')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Star, "*", this.lineNumber);
                }
                if (currentChar == '/')
                {
                    this.ConsumeChar();

                    if (this._input[this.position] == '/')
                    {
                        this.ConsumeComment();
                        return this.NextToken();
                    }

                    if (this._input[this.position] == '*')
                    {

                        this.ConsumeBlockComment();
                        return this.NextToken();
                    }

                    return new Token(TokenId.Slash, "/", this.lineNumber);
                }

                if (currentChar == '(')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.LeftParen, "(", this.lineNumber);
                }
                if (currentChar == ')')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.RightParen, ")", this.lineNumber);
                }
                if (currentChar == '{')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.LeftBrace, "{", this.lineNumber);
                }
                if (currentChar == '}')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.RightBrace, "}", this.lineNumber);
                }

                if (currentChar == '[')
                {
                    this.ConsumeChar();
                    if (this._input[this.position] == ']')
                    {
                        throw new SyntaxException($"Line {this.lineNumber + 1}. Value expected.");
                    }

                    return new Token(TokenId.LeftSquareBracket, "[", this.lineNumber);
                }

                if (currentChar == ']')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.RightSquareBracket, "]", this.lineNumber);
                }

                if (currentChar == ',')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Comma, ",", this.lineNumber);
                }

                if (currentChar == ';')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Semicolon, ";", this.lineNumber);
                }

                if (currentChar == '?')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Question, "?", this.lineNumber);
                }

                if (currentChar == ':')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Colon, ":", this.lineNumber);
                }

                if (currentChar == '=')
                {
                    this.ConsumeChar();
                    if (this._input[this.position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.EqualEqual, "==", this.lineNumber);
                    }

                    return new Token(TokenId.Equal, "=", this.lineNumber);
                }

                if (currentChar == '>')
                {
                    this.ConsumeChar();
                    if (this._input[this.position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.GreaterEqual, ">=", this.lineNumber);
                    }

                    return new Token(TokenId.Greater, ">", this.lineNumber);
                }

                if (currentChar == '<')
                {
                    this.ConsumeChar();
                    if (this._input[this.position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.LessEqual, "<=", this.lineNumber);
                    }

                    return new Token(TokenId.Less, "<", this.lineNumber);
                }

                if (currentChar == '!')
                {
                    this.ConsumeChar();
                    if (this._input[this.position] == '=')
                    {
                        this.ConsumeChar();
                        return new Token(TokenId.BangEqual, "!=", this.lineNumber);
                    }

                    return new Token(TokenId.Bang, "!", this.lineNumber);
                }


                if (currentChar == '.')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Dot, ".", this.lineNumber);
                }

                if (currentChar == '%')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.Percent, "%", this.lineNumber);
                }
                // Numbers
                if (char.IsDigit(currentChar))
                {
                    return new Token(TokenId.Number, this.ConsumeNumber(), this.lineNumber);
                }

                if (currentChar == '"' || currentChar == '\'')
                {
                    this.ConsumeChar();
                    return new Token(TokenId.String, this.ConsumeString(currentChar), this.lineNumber);
                }

                // Identifiers
                if (char.IsLetter(currentChar))
                {
                    var word = this.ConsumeIdentifier();
                    if (word == "if") { return new Token(TokenId.If, null, this.lineNumber); }
                    if (word == "else") { return new Token(TokenId.Else, null, this.lineNumber); }
                    if (word == "for") { return new Token(TokenId.For, null, this.lineNumber); }
                    if (word == "var") { return new Token(TokenId.Var, null, this.lineNumber); }
                    if (word == "return") { return new Token(TokenId.Return, null, this.lineNumber); }
                    if (word == "true") { return new Token(TokenId.True, null, this.lineNumber); }
                    if (word == "false") { return new Token(TokenId.False, null, this.lineNumber); }

                    if (this.functionNames.Contains(word))
                    {
                        return new Token(TokenId.Function, word, this.lineNumber);
                    }

                    return new Token(TokenId.Identifier, word, this.lineNumber);
                }

                // Invalid character
                throw new SyntaxException($"Line {this.lineNumber + 1}. Invalid character '{currentChar}' at position {this.position}");
            }

            return new Token(TokenId.Eof, string.Empty, this.lineNumber);
        }

        private void ConsumeChar()
        {
            this.position++;
        }

        private string ConsumeNumber()
        {
            var start = this.position;

            while (this.position < this._input.Length && (char.IsDigit(this._input[this.position]) || this._input[this.position] == '.'))
            {
                this.position++;
            }

            return this._input.Substring(start, this.position - start);
        }

        private string ConsumeIdentifier()
        {
            var start = this.position;
            while (this.position < this._input.Length && (char.IsLetterOrDigit(this._input[this.position]) || this._input[this.position] == '_'))
            {
                this.position++;
            }
            return this._input.Substring(start, this.position - start);
        }

        private string ConsumeString(char quote)
        {
            var start = this.position;
            while (this.position < this._input.Length && this._input[this.position] != quote)
            {
                this.position++;
            }

            return (this.position < this._input.Length && this._input[this.position] == quote)
                ? this._input[start..this.position++]
                : throw new ScriptException($"Line {this.lineNumber + 1}. {quote} expected.");
        }

        private void ConsumeComment()
        {
            while (this.position < this._input.Length)
            {
                if (this._input[this.position++] == '\n')
                {
                    this.lineNumber++;
                    return;
                }
            }
        }

        private void ConsumeBlockComment()
        {
            this.position++;
            while (this.position < this._input.Length)
            {
                var character = this._input[this.position++];
                if (character == '\n') { this.lineNumber++; }
                if (character == '*' && this.position < this._input.Length)
                {
                    var nextCharacter = this._input[this.position++];
                    if (nextCharacter == '/')
                    {
                        return;
                    }
                }
            }

            throw new ScriptException(Constants.ExceptionMessages.UnterminatedBlockComment);
        }
    }
}
