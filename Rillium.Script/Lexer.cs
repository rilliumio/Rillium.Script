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
            _input = input;
            _position = 0;
            this.functionNames = functionNames;
        }

        public Token NextToken()
        {
            while (_position < _input.Length)
            {
                var currentChar = _input[_position];

                // Whitespace
                if (char.IsWhiteSpace(currentChar))
                {
                    if (currentChar == '\n') { _lineNumber++; }
                    ConsumeChar();
                    continue;
                }

                // Operators and delimiters
                if (currentChar == '+')
                {
                    ConsumeChar();
                    return new Token(TokenId.Plus, "+", _lineNumber);
                }
                if (currentChar == '-')
                {
                    ConsumeChar();
                    return new Token(TokenId.Minus, "-", _lineNumber);
                }
                if (currentChar == '*')
                {
                    ConsumeChar();
                    return new Token(TokenId.Star, "*", _lineNumber);
                }
                if (currentChar == '/')
                {
                    ConsumeChar();
                    return new Token(TokenId.Slash, "/", _lineNumber);
                }
                if (currentChar == '(')
                {
                    ConsumeChar();
                    return new Token(TokenId.LeftParen, "(", _lineNumber);
                }
                if (currentChar == ')')
                {
                    ConsumeChar();
                    return new Token(TokenId.RightParen, ")", _lineNumber);
                }
                if (currentChar == '{')
                {
                    ConsumeChar();
                    return new Token(TokenId.LeftBrace, "{", _lineNumber);
                }
                if (currentChar == '}')
                {
                    ConsumeChar();
                    return new Token(TokenId.RightBrace, "}", _lineNumber);
                }

                if (currentChar == '[')
                {
                    ConsumeChar();
                    return new Token(TokenId.LeftSquareBracket, "[", _lineNumber);
                }

                if (currentChar == ']')
                {
                    ConsumeChar();
                    return new Token(TokenId.RightSquareBracket, "[", _lineNumber);
                }

                if (currentChar == ',')
                {
                    ConsumeChar();
                    return new Token(TokenId.Comma, ",", _lineNumber);
                }

                if (currentChar == ';')
                {
                    ConsumeChar();
                    return new Token(TokenId.Semicolon, ";", _lineNumber);
                }

                if (currentChar == '=')
                {
                    ConsumeChar();
                    if (_input[_position] == '=')
                    {
                        ConsumeChar();
                        return new Token(TokenId.EqualEqual, "==", _lineNumber);
                    }

                    return new Token(TokenId.Equal, "=", _lineNumber);
                }

                if (currentChar == '>')
                {
                    ConsumeChar();
                    if (_input[_position] == '=')
                    {
                        ConsumeChar();
                        return new Token(TokenId.GreaterEqual, ">=", _lineNumber);
                    }

                    return new Token(TokenId.Greater, ">", _lineNumber);
                }

                if (currentChar == '<')
                {
                    ConsumeChar();
                    if (_input[_position] == '=')
                    {
                        ConsumeChar();
                        return new Token(TokenId.LessEqual, "<=", _lineNumber);
                    }

                    return new Token(TokenId.Less, "<", _lineNumber);
                }


                if (currentChar == '.')
                {
                    ConsumeChar();
                    return new Token(TokenId.Dot, ".", _lineNumber);
                }


                // Numbers
                if (char.IsDigit(currentChar))
                {
                    return new Token(TokenId.Number, ConsumeNumber(), _lineNumber);
                }

                // Identifiers
                if (char.IsLetter(currentChar))
                {
                    var word = ConsumeIdentifier();
                    if (word == "if") { return new Token(TokenId.If, null, _lineNumber); }
                    if (word == "else") { return new Token(TokenId.Else, null, _lineNumber); }
                    if (word == "for") { return new Token(TokenId.For, null, _lineNumber); }
                    if (word == "var") { return new Token(TokenId.Var, null, _lineNumber); }
                    if (word == "return") { return new Token(TokenId.Return, null, _lineNumber); }

                    if (functionNames.Contains(word))
                    {
                        return new Token(TokenId.Function, word, _lineNumber);
                    }

                    return new Token(TokenId.Identifier, word, _lineNumber);
                }

                // Invalid character
                throw new ArgumentException($"Invalid character '{currentChar}' at position {_position}");
            }

            return new Token(TokenId.Eof, string.Empty, _lineNumber);
        }

        private void ConsumeChar()
        {
            _position++;
        }

        private string ConsumeNumber()
        {
            var start = _position;
            var hasPeriod = false;

            while (_position < _input.Length && (char.IsDigit(_input[_position]) || _input[_position] == '.'))
            {
                if (_input[_position] == '.')
                {
                    if (hasPeriod)
                    {
                        throw new ArgumentException("Invalid number.");
                    }

                    hasPeriod = true;
                }

                _position++;
            }

            return _input.Substring(start, _position - start);
        }

        private string ConsumeIdentifier()
        {
            var start = _position;
            while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '_'))
            {
                _position++;
            }
            return _input.Substring(start, _position - start);
        }
    }

}
