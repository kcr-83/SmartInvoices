# AI Rules for SmartInvestor Project
Cały opis projektu znajduje się w plikach w folderze `.github/instructions/project/`. Proszę przeczytać te pliki, aby uzyskać pełny kontekst projektu. 
Główny opis fukncjonalności znajduje się w pliku `feature-requirement.md`, który zawiera wymagania biznesowe i techniczne.
Proszę również zwrócić uwagę na plik `project-structure.md`, który zawiera szczegółową strukturę projektu, w tym podział na warstwy i moduły.
Wiedza domenowa znajduje się w pliku `domain-knowledge.md`, który zawiera informacje o kontekście biznesowym i wymaganiach funkcjonalnych.

Wprowadzając zmiany w projekcie, proszę również aktualizować odpowiednie pliki w folderze `.github/instructions/project/`, aby zachować spójność dokumentacji.

W pliku 'project/example-dotnet-catalogs.md' znajdują się przykłady katalogów dla aplikacji .NET, które mogą być przydatne podczas tworzenia nowych modułów lub funkcjonalności a takze poprawny opis jak powinna wygladać aplikacja .NET w architekturze Clean Architecture.

Dla aplikacji Angularowych użyj instrukcji zawartych w katalogu `.github/instructions/angular/`.

Dla aplikacji .Net użyj instrukcji zawartych w katalogu `.github/instructions/dotnet/`.

Dla bazydanych SQL użyj instrukcji zawartych w katalogu `.github/instructions/database/`.

Dla testów użyj instrukcji zawartych w katalogu `.github/instructions/tests/`.

## CODING_PRACTICES

### Guidelines for SUPPORT_LEVEL

#### SUPPORT_BEGINNER

- When running in agent mode, execute up to 3 actions at a time and ask for approval or course correction afterwards.
- Write code with clear variable names and include explanatory comments for non-obvious logic. Avoid shorthand syntax and complex patterns.
- Provide full implementations rather than partial snippets. Include import statements, required dependencies, and initialization code.
- Add defensive coding patterns and clear error handling. Include validation for user inputs and explicit type checking.
- Suggest simpler solutions first, then offer more optimized versions with explanations of the trade-offs.
- Briefly explain why certain approaches are used and link to relevant documentation or learning resources.
- When suggesting fixes for errors, explain the root cause and how the solution addresses it to build understanding. Ask for confirmation before proceeding.
- Offer introducing basic test cases that demonstrate how the code works and common edge cases to consider.

### Guidelines for DOCUMENTATION

#### SWAGGER

- Define comprehensive schemas for all request and response objects
- Use semantic versioning in API paths to maintain backward compatibility
- Implement detailed descriptions for endpoints, parameters, and {{domain_specific_concepts}}
- Configure security schemes to document authentication and authorization requirements
- Use tags to group related endpoints by resource or functional area
- Implement examples for all endpoints to facilitate easier integration by consumers

### Guidelines for ARCHITECTURE

#### ADR

- Create ADRs in /docs/adr/{name}.md for:
- 1) Major dependency changes
- 2) Architectural pattern changes
- 3) New integration patterns
- 4) Database schema changes

#### CLEAN_ARCHITECTURE

- Strictly separate code into layers: entities, use cases, interfaces, and frameworks
- Ensure dependencies point inward, with inner layers having no knowledge of outer layers
- Implement domain entities that encapsulate {{business_rules}} without framework dependencies
- Use interfaces (ports) and implementations (adapters) to isolate external dependencies
- Create use cases that orchestrate entity interactions for specific business operations
- Implement mappers to transform data between layers to maintain separation of concerns


#### DDD

- Define bounded contexts to separate different parts of the domain with clear boundaries
- Implement ubiquitous language within each context to align code with business terminology
- Create rich domain models with behavior, not just data structures, for core business logic
- Use value objects for concepts with no identity but defined by their attributes
- Implement domain events to communicate between bounded contexts
- Use aggregates to enforce consistency boundaries and transactional integrity

<!-- ### Guidelines for STATIC_ANALYSIS

#### ESLINT

- Configure project-specific rules in eslint.config.js to enforce consistent coding standards
- Use shareable configs like eslint-config-airbnb or eslint-config-standard as a foundation
- Implement custom rules for {{project_specific_patterns}} to maintain codebase consistency
- Configure integration with Prettier to avoid rule conflicts for code formatting
- Use the --fix flag in CI/CD pipelines to automatically correct fixable issues
- Implement staged linting with husky and lint-staged to prevent committing non-compliant code

#### PRETTIER

- Define a consistent .prettierrc configuration across all projects to enforce code style
- Configure editor integration to format on save for immediate feedback
- Use .prettierignore to exclude generated files, build artifacts
- Set printWidth based on team preferences (80-120 characters) to improve code readability
- Configure consistent quote style and semicolon usage to match team conventions
- Implement CI checks to ensure all committed code adheres to the defined style -->