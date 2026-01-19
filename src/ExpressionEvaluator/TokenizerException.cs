namespace ExpressionEvaluator;

public sealed class TokenizerException : Exception
{
    public int Position { get; }

    public TokenizerException(string message, int position)
        : base(message)
    {
        Position = position;
    }
}
