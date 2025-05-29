# Complexity Hotspot Refactor

Your goal is to identify and explain complexity hotspots in the provided code.

## Requirements

- Analyze the code and list functions/methods ordered by cyclomatic complexity (highest first).
- For each function/method, explain why it is risky (e.g., deep nesting, duplication, side effects).
- Suggest quick wins for simplification (e.g., extract functions, reduce nesting, remove duplication).
- Use clear, concise language and markdown formatting.
- Follow the [Project Naming Guideline](../project-naming-guideline.md) for any naming suggestions.

## Output Format

```
1. functionName (complexity: 8)
   - Risk: Deep nesting and multiple side effects.
   - Quick win: Extract validation logic into a helper function.
2. anotherFunction (complexity: 5)
   - Risk: Code duplication.
   - Quick win: Reuse a shared utility function.
```
