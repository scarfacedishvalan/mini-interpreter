using ExpressionEvaluator;

namespace ExpressionEvaluator.Tests;

public sealed class ParserTests
{
    [Fact]
    public void Parse_UnmatchedRightParen_Throws()
    {
        var tokens = new Tokenizer("1 + 2)").Tokenize();
        var parser = new Parser(tokens);

        var ex = Assert.Throws<ParseException>(() => parser.Parse());
        Assert.Contains("Unexpected token", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Parse_EmptyInput_Throws()
    {
        var tokens = new Tokenizer("").Tokenize();
        var parser = new Parser(tokens);

        var ex = Assert.Throws<ParseException>(() => parser.Parse());
        Assert.Contains("Expected", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
