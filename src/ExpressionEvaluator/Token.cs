namespace ExpressionEvaluator;

public enum TokenType
{
    Number,
    Plus,
    Minus,
    Star,
    Slash,
    LeftParen,
    RightParen,
    EndOfInput,
}

public readonly record struct Token(
    TokenType Type,
    string Lexeme,
    int Position,
    double? NumberValue = null
);
