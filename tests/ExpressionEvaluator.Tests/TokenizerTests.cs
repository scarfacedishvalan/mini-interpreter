using ExpressionEvaluator;

namespace ExpressionEvaluator.Tests;

public sealed class TokenizerTests
{
    [Fact]
    public void Tokenize_IgnoresWhitespace()
    {
        var tokenizer = new Tokenizer("  3  +\t4\n");
        var tokens = tokenizer.Tokenize();

        Assert.Equal(TokenType.Number, tokens[0].Type);
        Assert.Equal(3.0, tokens[0].NumberValue);

        Assert.Equal(TokenType.Plus, tokens[1].Type);

        Assert.Equal(TokenType.Number, tokens[2].Type);
        Assert.Equal(4.0, tokens[2].NumberValue);

        Assert.Equal(TokenType.EndOfInput, tokens[^1].Type);
    }

    [Fact]
    public void Tokenize_InvalidCharacter_ReportsPosition()
    {
        var ex = Assert.Throws<TokenizerException>(() => new Tokenizer("1 + @").Tokenize());
        Assert.Equal(4, ex.Position);
        Assert.Contains("position 4", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Tokenize_InvalidNumberLiteral_Throws()
    {
        Assert.Throws<TokenizerException>(() => new Tokenizer(".").Tokenize());
    }
}
