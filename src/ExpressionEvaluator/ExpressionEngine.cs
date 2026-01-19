namespace ExpressionEvaluator;

public static class ExpressionEngine
{
    public static double Evaluate(string expression)
    {
        var tokenizer = new Tokenizer(expression ?? string.Empty);
        var tokens = tokenizer.Tokenize();

        var parser = new Parser(tokens);
        var ast = parser.Parse();

        var evaluator = new Evaluator();
        return evaluator.Evaluate(ast);
    }
}
