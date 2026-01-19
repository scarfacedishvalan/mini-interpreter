using ExpressionEvaluator;

Console.WriteLine("Enter expression:");
var input = Console.ReadLine();

try
{
	var result = ExpressionEngine.Evaluate(input ?? string.Empty);
	Console.WriteLine();
	Console.WriteLine($"Expression: {input}");
	Console.WriteLine($"Result: {result}");
}
catch (TokenizerException ex)
{
	Console.WriteLine();
	Console.WriteLine($"Tokenization error: {ex.Message}");
}
catch (ParseException ex)
{
	Console.WriteLine();
	Console.WriteLine($"Parse error: {ex.Message}");
}
catch (EvaluationException ex)
{
	Console.WriteLine();
	Console.WriteLine($"Math error: {ex.Message}");
}
catch (Exception ex)
{
	Console.WriteLine();
	Console.WriteLine($"Unexpected error: {ex.Message}");
}
