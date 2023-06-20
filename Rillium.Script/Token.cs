namespace Rillium.Script
{
    public class Token
    {
        public TokenId Id { get; }
        public string? Value { get; }
        public int Line { get; }

        public TokenId? PreToken { get; set; }

        public Token(TokenId tokenId, string? value, int line)
        {
            this.Id = tokenId;
            this.Value = value;
            this.Line = line;
        }


        public override string ToString() => $"{this.Id}: {this.Value}";
    }
}
