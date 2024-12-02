namespace Rillium.Script
{
    public enum TokenId
    {
        // Single-character tokens
        LeftParen, RightParen, LeftBrace, RightBrace, LeftSquareBracket, RightSquareBracket,
        Comma, Dot, Minus, Plus, Semicolon, Slash, Star, Percent, Question, Colon,

        // One or two character tokens
        Bang, BangEqual,
        Equal, EqualEqual,
        Greater, GreaterEqual,
        Less, LessEqual, PlusPlus, MinusMinus,

        // Literals
        Identifier, String, Number, Function,

        // Keywords
        And, Else, False, For, If, Nil, Or,
        Print, Return, True, Var,

        // End of file
        Eof
    }
}
