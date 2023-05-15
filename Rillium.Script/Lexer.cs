namespace Rillium.Script
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
        }

        public Token NextToken()
        {
            while (_position < _input.Length)
            {
                var currentChar = _input[_position];

                // Whitespace
                if (char.IsWhiteSpace(currentChar))
                {
                    ConsumeChar();
                    continue;
                }

                // Operators and delimiters
                if (currentChar == '+')
                {
                    ConsumeChar();
                    return new Token(TokenType.Plus, "+");
                }
                if (currentChar == '-')
                {
                    ConsumeChar();
                    return new Token(TokenType.Minus, "-");
                }
                if (currentChar == '*')
                {
                    ConsumeChar();
                    return new Token(TokenType.Star, "*");
                }
                if (currentChar == '/')
                {
                    ConsumeChar();
                    return new Token(TokenType.Slash, "/");
                }
                if (currentChar == '(')
                {
                    ConsumeChar();
                    return new Token(TokenType.LeftParen, "(");
                }
                if (currentChar == ')')
                {
                    ConsumeChar();
                    return new Token(TokenType.RightParen, ")");
                }
                if (currentChar == '{')
                {
                    ConsumeChar();
                    return new Token(TokenType.LeftBrace, "{");
                }
                if (currentChar == '}')
                {
                    ConsumeChar();
                    return new Token(TokenType.RightBrace, "}");
                }

                if (currentChar == '[')
                {
                    ConsumeChar();
                    return new Token(TokenType.LeftSquareBracket, "[");
                }

                if (currentChar == ']')
                {
                    ConsumeChar();
                    return new Token(TokenType.RightSquareBracket, "[");
                }

                if (currentChar == ',')
                {
                    ConsumeChar();
                    return new Token(TokenType.Comma, ",");
                }

                if (currentChar == ';')
                {
                    ConsumeChar();
                    return new Token(TokenType.Semicolon, ";");
                }

                if (currentChar == '=')
                {
                    ConsumeChar();
                    if (_input[_position] == '=')
                    {
                        ConsumeChar();
                        return new Token(TokenType.EqualEqual, "==");
                    }

                    return new Token(TokenType.Equal, "=");
                }

                if (currentChar == '>')
                {
                    ConsumeChar();
                    if (_input[_position] == '=')
                    {
                        ConsumeChar();
                        return new Token(TokenType.GreaterEqual, ">=");
                    }

                    return new Token(TokenType.Greater, ">");
                }

                if (currentChar == '<')
                {
                    ConsumeChar();
                    if (_input[_position] == '=')
                    {
                        ConsumeChar();
                        return new Token(TokenType.LessEqual, "<=");
                    }

                    return new Token(TokenType.Less, "<");
                }


                // Numbers
                if (char.IsDigit(currentChar))
                {
                    return new Token(TokenType.Number, ConsumeNumber());
                }

                // Identifiers
                if (char.IsLetter(currentChar))
                {
                    var word = ConsumeIdentifier();
                    if (word == "if") { return new Token(TokenType.If, null); }
                    if (word == "else") { return new Token(TokenType.Else, null); }
                    if (word == "for") { return new Token(TokenType.For, null); }
                    if (word == "var") { return new Token(TokenType.Var, null); }
                    if (word == "return") { return new Token(TokenType.Return, null); }

                    return new Token(TokenType.Identifier, word);
                }

                // Invalid character
                throw new ArgumentException($"Invalid character '{currentChar}' at position {_position}");
            }

            return new Token(TokenType.Eof, string.Empty);
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
