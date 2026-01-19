namespace ExpressionEvaluator;

public sealed class Evaluator
{
    private const double DivisionEpsilon = 1e-12;

    public double Evaluate(AstNode node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return node switch
        {
            NumberNode n => n.Value,
            BinaryExpressionNode b => EvaluateBinary(b),
            _ => throw new EvaluationException($"Unsupported AST node type '{node.GetType().Name}'."),
        };
    }

    private double EvaluateBinary(BinaryExpressionNode node)
    {
        var left = Evaluate(node.Left);
        var right = Evaluate(node.Right);

        return node.Operator switch
        {
            BinaryOperator.Add => left + right,
            BinaryOperator.Subtract => left - right,
            BinaryOperator.Multiply => left * right,
            BinaryOperator.Divide => Divide(left, right),
            _ => throw new EvaluationException($"Unsupported binary operator '{node.Operator}'."),
        };
    }

    private static double Divide(double left, double right)
    {
        if (Math.Abs(right) < DivisionEpsilon)
        {
            throw new EvaluationException("Division by zero.");
        }

        return left / right;
    }
}
