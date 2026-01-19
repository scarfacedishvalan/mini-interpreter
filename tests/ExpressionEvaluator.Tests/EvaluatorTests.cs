using ExpressionEvaluator;

namespace ExpressionEvaluator.Tests;

public sealed class EvaluatorTests
{
    [Fact]
    public void Evaluate_InvalidAstNode_ThrowsMeaningfulException()
    {
        var evaluator = new Evaluator();
        var ex = Assert.Throws<EvaluationException>(() => evaluator.Evaluate(new FakeNode()));
        Assert.Contains("Unsupported", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class FakeNode : AstNode
    {
    }
}
