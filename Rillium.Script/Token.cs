namespace Rillium.Script
{
    public class Token
    {
        public TokenId Id { get; }
        public string Value { get; }
        public int Arguments { get; }
        public int Line { get; }

        //public Token(TokenType type, string value)
        //{
        //    Type = type;
        //    Value = value;
        //}

        public Token(TokenId tokenId, string value, int line)
        {
            Id = tokenId;
            Value = value;
            Line = line;
        }

        public override string ToString()
        {
            return $"{Id}: {Value}";
        }
    }

}
