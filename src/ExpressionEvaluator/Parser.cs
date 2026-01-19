namespace ExpressionEvaluator;

public sealed class Parser
{
    private readonly IReadOnlyList<Token> _tokens;
    private int _index;

    public Parser(IReadOnlyList<Token> tokens)
    {
        _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
        _index = 0;
    }

    public AstNode Parse()
    {
        var expr = ParseExpression();

        if (Current.Type != TokenType.EndOfInput)
        {
            throw new ParseException($"Unexpected token '{Current.Lexeme}' at position {Current.Position}.", Current.Position);
        }

        return expr;
    }

    // Expression -> Factor ((*|/) Factor)*
    private AstNode ParseExpression()
    {
        var left = ParseFactor();

        while (Current.Type is TokenType.Star or TokenType.Slash)
        {
            var op = Consume(Current.Type);
            var right = ParseFactor();

            left = new BinaryExpressionNode(
                left,
                op.Type == TokenType.Star ? BinaryOperator.Multiply : BinaryOperator.Divide,
                right);
        }

        return left;
    }

    // Term -> Expression ((+|-) Expression)*
    private AstNode ParseTerm()
    {
        var left = ParseExpression();

        while (Current.Type is TokenType.Plus or TokenType.Minus)
        {
            var op = Consume(Current.Type);
            var right = ParseExpression();

            left = new BinaryExpressionNode(
                left,
                op.Type == TokenType.Plus ? BinaryOperator.Add : BinaryOperator.Subtract,
                right);
        }

        return left;
    }

    // Factor -> ('+' | '-')? (Number | '(' Expression ')')
    private AstNode ParseFactor()
    {
        if (Current.Type == TokenType.Plus)
        {
            Consume(TokenType.Plus);
            // Unary plus: no-op
            return ParseFactor();
        }

        if (Current.Type == TokenType.Minus)
        {
            Consume(TokenType.Minus);
            // Unary minus: multiply by -1
            return new BinaryExpressionNode(
                ParseFactor(),
                BinaryOperator.Multiply,
                new NumberNode(-1));
        }

        if (Current.Type == TokenType.Number)
        {
            var number = Consume(TokenType.Number);
            if (number.NumberValue is null)
            {
                throw new ParseException($"Invalid number token at position {number.Position}.", number.Position);
            }

            return new NumberNode(number.NumberValue.Value);
        }

        if (Current.Type == TokenType.LeftParen)
        {
            var leftParen = Consume(TokenType.LeftParen);
            var expr = ParseExpression();

            if (Current.Type != TokenType.RightParen)
            {
                throw new ParseException($"Unmatched '(' at position {leftParen.Position}; expected ')'.", leftParen.Position);
            }

            Consume(TokenType.RightParen);
            return expr;
        }

        throw new ParseException($"Expected number or '(' at position {Current.Position} but found '{Current.Lexeme}'.", Current.Position);
    }

    private Token Current => _tokens[_index];

    private Token Consume(TokenType expected)
    {
        if (Current.Type != expected)
        {
            throw new ParseException(
                $"Expected token {expected} at position {Current.Position} but found '{Current.Lexeme}'.",
                Current.Position);
        }

        var token = Current;
        _index = Math.Min(_index + 1, _tokens.Count - 1);
        return token;
    }
}
