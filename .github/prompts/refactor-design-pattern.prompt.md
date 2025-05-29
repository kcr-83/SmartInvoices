# Design Pattern Refactor: Strategy Pattern

Your goal is to refactor code that violates the Open-Closed Principle due to repeated `switch` or `if` statements on a type or discriminator.

## Requirements

- Identify the module and the type/discriminator causing repeated branching.
- Refactor using the Strategy pattern:
  - Define an interface for the behavior.
  - Implement concrete strategy classes for each type.
  - Create a factory or registry to resolve the correct strategy by type.
- Keep the public API unchanged.
- Ensure all logic previously in the switch/if is now encapsulated in strategies.
- Add TSDoc comments to all new classes and interfaces.

## Output

- Provide the full refactored code block.
- Include a brief explanation of the changes and how the Strategy pattern is applied.
