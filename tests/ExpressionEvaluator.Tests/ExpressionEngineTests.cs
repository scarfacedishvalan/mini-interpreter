using ExpressionEvaluator;

namespace ExpressionEvaluator.Tests;

public sealed class ExpressionEngineTests
{
    [Theory]
    [InlineData("1", 1.0)]
    [InlineData("42", 42.0)]
    [InlineData("3.14", 3.14)]
    [InlineData("0.5", 0.5)]
    [InlineData(".5", 0.5)]
    [InlineData("3 + 5 * 2", 13.0)]
    [InlineData("(3 + 5) * 2", 16.0)]
    [InlineData("10 - 4 - 3", 3.0)]
    [InlineData("10 / 2 * 5", 25.0)]
    [InlineData("  \t\n  3   +\n  5\t* 2  ", 13.0)]
    [InlineData("((2))", 2.0)]
    public void Evaluate_ValidExpressions_ReturnExpected(string expression, double expected)
    {
        var actual = ExpressionEngine.Evaluate(expression);
        Assert.Equal(expected, actual, precision: 10);
    }

    [Fact]
    public void Evaluate_DeepParentheses_ReturnExpected()
    {
        // 100 levels of nesting
        var expr = "1";
        for (var i = 0; i < 100; i++)
        {
            expr = $"({expr})";
        }

        var actual = ExpressionEngine.Evaluate(expr);
        Assert.Equal(1.0, actual, precision: 10);
    }

    [Fact]
    public void Evaluate_LargeNumbers_ReturnExpected()
    {
        var actual = ExpressionEngine.Evaluate("9999999999999999 + 1");
        // Double precision limitations apply; this asserts we produce a finite result and don’t throw.
        Assert.True(double.IsFinite(actual));
    }

    [Theory]
    [InlineData("3 +")]
    [InlineData("(3 + 2")]
    [InlineData("3 + * 4")]
    [InlineData("3 ++ 4")]
    [InlineData("3 4")]
    [InlineData("()")]
    public void Evaluate_InvalidExpressions_ThrowParseException(string expression)
    {
        var ex = Assert.Throws<ParseException>(() => ExpressionEngine.Evaluate(expression));
        Assert.False(string.IsNullOrWhiteSpace(ex.Message));
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("3 + a")]
    [InlineData("2 & 3")]
    public void Evaluate_InvalidCharacters_ThrowTokenizerException(string expression)
    {
        var ex = Assert.Throws<TokenizerException>(() => ExpressionEngine.Evaluate(expression));
        Assert.Contains("Invalid character", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_DivisionByZero_ThrowsEvaluationException()
    {
        var ex = Assert.Throws<EvaluationException>(() => ExpressionEngine.Evaluate("1 / 0"));
        Assert.Contains("Division by zero", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
