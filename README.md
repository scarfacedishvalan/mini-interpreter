# Mini Expression Evaluator (C# / .NET)

A small, dependency-free arithmetic expression interpreter written in C#.
It **tokenizes**, **parses** (recursive descent), and **evaluates** arithmetic expressions into a `double`.

## Supported Features

- Numeric literals
  - Integers: `1`, `42`
  - Floating point: `3.14`, `0.5`, `.5`
- Binary operators: `+`, `-`, `*`, `/`
- Parentheses: `(`, `)`
- Whitespace anywhere in the expression
- Correct precedence and left-associativity
b
Examples:

- `3 + 5 * 2` → `13`
- `(3 + 5) * 2` → `16`
- `10 - 4 - 3` → `3`
- `10 / 2 * 5` → `25`

## Architecture

The solution is split into small, focused components:

- **Tokenizer** ([src/ExpressionEvaluator/Tokenizer.cs](src/ExpressionEvaluator/Tokenizer.cs))
  - Converts the input string into a list of tokens.
  - Recognizes numbers, operators, parentheses.
  - Ignores whitespace.
  - Throws `TokenizerException` with a clear message and character position for invalid input.

- **Parser** ([src/ExpressionEvaluator/Parser.cs](src/ExpressionEvaluator/Parser.cs))
  - Recursive descent parser that builds an **AST**.
  - Implements operator precedence via grammar layering:

    - `Expression -> Term ((+|-) Term)*`
    - `Term -> Factor ((*|/) Factor)*`
    - `Factor -> Number | '(' Expression ')'`

  - Throws `ParseException` with a clear message and position for invalid sequences or unmatched parentheses.

- **AST** ([src/ExpressionEvaluator/AstNode.cs](src/ExpressionEvaluator/AstNode.cs))
  - Immutable node types:
    - `AstNode` (base)
    - `NumberNode`
    - `BinaryExpressionNode`

- **Evaluator** ([src/ExpressionEvaluator/Evaluator.cs](src/ExpressionEvaluator/Evaluator.cs))
  - Walks the AST and produces a `double`.
  - Throws `EvaluationException` for math/runtime issues (e.g., division by zero).

- **Public entrypoint** ([src/ExpressionEvaluator/ExpressionEngine.cs](src/ExpressionEvaluator/ExpressionEngine.cs))
  - `ExpressionEngine.Evaluate(string expression)` ties it all together.

## Why Recursive Descent?

- It is easy to read and maintain.
- It naturally expresses precedence through layered parsing functions (Expression/Term/Factor).
- It produces an explicit AST, which is easy to extend later (e.g., unary operators, functions).

Tradeoffs:
- Very deep nesting could hit recursion limits (acceptable for a mini interpreter).
- Error recovery is simple (fail fast with clear exceptions), which is desired here.

## How to Run

### Prerequisites

- .NET SDK 8+ installed

### Build

From the repo root:

```bash
dotnet build
```

### Run

```bash
dotnet run --project src/ExpressionEvaluator
```

Example session:

```text
Enter expression:
(3 + 5) * 2

Result: 16
```

## How to Run Tests

```bash
dotnet test
```

Expected output (summary will vary by machine):

- All tests should pass.
- Failures should report the specific expression and the expected behavior.

## Project Layout

- `ExpressionEvaluator.slnx` – solution
- `src/ExpressionEvaluator/` – console app + core interpreter code
- `tests/ExpressionEvaluator.Tests/` – xUnit tests
