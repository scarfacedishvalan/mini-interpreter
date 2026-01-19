namespace ExpressionEvaluator;

public abstract class AstNode
{
}

public sealed class NumberNode : AstNode
{
    public double Value { get; }

    public NumberNode(double value)
    {
        Value = value;
    }
}

public enum BinaryOperator
{
    Add,
    Subtract,
    Multiply,
    Divide,
}

public sealed class BinaryExpressionNode : AstNode
{
    public AstNode Left { get; }
    public AstNode Right { get; }
    public BinaryOperator Operator { get; }

    public BinaryExpressionNode(AstNode left, BinaryOperator @operator, AstNode right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Right = right ?? throw new ArgumentNullException(nameof(right));
        Operator = @operator;
    }
}
