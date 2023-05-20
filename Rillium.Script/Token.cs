namespace Rillium.Script
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Arguments { get; }
        public int Line { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public Token(TokenType type, string value, int line)
        {
            Type = type;
            Value = value;
            Line = line;
        }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }

}
