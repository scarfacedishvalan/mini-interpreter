using System.Globalization;

namespace ExpressionEvaluator;

public sealed class Tokenizer
{
    private readonly string _text;
    private int _index;

    public Tokenizer(string text)
    {
        _text = text ?? throw new ArgumentNullException(nameof(text));
        _index = 0;
    }

    public IReadOnlyList<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (!IsAtEnd())
        {
            SkipWhitespace();
            if (IsAtEnd())
            {
                break;
            }

            var ch = Peek();
            var position = _index;

            if (char.IsDigit(ch) || (ch == '.' && char.IsDigit(PeekNext())))
            {
                tokens.Add(ReadNumber());
                continue;
            }

            _index++;
            tokens.Add(ch switch
            {
                '+' => new Token(TokenType.Plus, "+", position),
                '-' => new Token(TokenType.Minus, "-", position),
                '*' => new Token(TokenType.Star, "*", position),
                '/' => new Token(TokenType.Slash, "/", position),
                '(' => new Token(TokenType.LeftParen, "(", position),
                ')' => new Token(TokenType.RightParen, ")", position),
                _ => throw new TokenizerException($"Invalid character '{ch}' at position {position}.", position),
            });
        }

        tokens.Add(new Token(TokenType.EndOfInput, string.Empty, _index));
        return tokens;
    }

    private Token ReadNumber()
    {
        var start = _index;
        var seenDot = false;
        var digits = 0;

        while (!IsAtEnd())
        {
            var ch = Peek();

            if (char.IsDigit(ch))
            {
                digits++;
                _index++;
                continue;
            }

            if (ch == '.')
            {
                if (seenDot)
                {
                    break;
                }

                seenDot = true;
                _index++;
                continue;
            }

            break;
        }

        var lexeme = _text[start.._index];

        if (digits == 0)
        {
            throw new TokenizerException($"Invalid number literal '{lexeme}' at position {start}.", start);
        }

        try
        {
            var value = double.Parse(lexeme, NumberStyles.Float, CultureInfo.InvariantCulture);
            return new Token(TokenType.Number, lexeme, start, value);
        }
        catch (FormatException)
        {
            throw new TokenizerException($"Invalid number literal '{lexeme}' at position {start}.", start);
        }
        catch (OverflowException)
        {
            throw new TokenizerException($"Number literal '{lexeme}' is out of range at position {start}.", start);
        }
    }

    private void SkipWhitespace()
    {
        while (!IsAtEnd() && char.IsWhiteSpace(Peek()))
        {
            _index++;
        }
    }

    private bool IsAtEnd() => _index >= _text.Length;

    private char Peek() => _text[_index];

    private char PeekNext() => (_index + 1) < _text.Length ? _text[_index + 1] : '\0';
}
