namespace Rillium.Script
{
    public enum TokenId
    {

        // Single-character tokens
        LeftParen, RightParen, LeftBrace, RightBrace, LeftSquareBracket, RightSquareBracket,
        Comma, Dot, Minus, Plus, Semicolon, Slash, Star,

        // One or two character tokens
        Bang, BangEqual,
        Equal, EqualEqual,
        Greater, GreaterEqual,
        Less, LessEqual,

        // Literals
        Identifier, String, Number, Function,

        // Keywords
        And, Class, Else, False, Fun, For, If, Nil, Or,
        Print, Return, Super, This, True, Var, While,

        // End of file
        Eof
    }

}
