## Angular Architecture Best Practices

### Component Structure
- Follow the smart/presentational component pattern
  - Smart components handle data fetching and state management
  - Presentational components focus on UI rendering and user interactions
- Keep components focused on a single responsibility
- Use OnPush change detection strategy for performance optimization

### State Management
- Use NgRx for complex state management needs
- Use services with RxJS BehaviorSubjects for simpler state management
- Avoid using component properties for storing application state

### Data Flow
- Use Inputs for passing data to child components
- Use Outputs for notifying parent components of events
- Use services for sharing data between unrelated components

## Code Organization

### File and Folder Structure
- Follow the Angular style guide for file and folder organization
- Group related features in feature modules
- Use barrels (index.ts files) for cleaner imports

### Naming Conventions
- Files: `feature-name.component.ts`, `feature-name.service.ts`
- Selectors: `app-feature-name`
- Classes: FeatureNameComponent, FeatureNameService
- Interfaces: IFeatureName or FeatureNameInterface

## Performance Optimization

- Use OnPush change detection strategy
- Implement trackBy functions for ngFor directives
- Lazy load feature modules
- Use pure pipes instead of methods in templates
- Avoid unnecessary template binding expressions

## Testing Practices

- Write unit tests for all components and services
- Maintain high test coverage for critical features
- Use TestBed and component fixture for component testing
- Mock dependencies using jasmine spies

## Common Patterns

### Form Handling
- Use Reactive Forms for complex forms
- Implement proper form validation
- Create reusable form components

### HTTP Communication
- Centralize API calls in services
- Handle errors properly with catchError operators
- Use interceptors for common request/response handling

#### ANGULAR_CODING_STANDARDS

- Use standalone components, directives, and pipes instead of NgModules
- Implement signals for state management instead of traditional RxJS-based approaches
- Use the new inject function instead of constructor injection
- Implement control flow with @if, @for, and @switch instead of *ngIf, *ngFor, etc.
- Leverage functional guards and resolvers instead of class-based ones
- Use the new deferrable views for improved loading states
- Implement OnPush change detection strategy for improved performance
- Use TypeScript decorators with explicit visibility modifiers (public, private)
- Leverage Angular CLI for schematics and code generation
- create separate files for the component, view, and styles, e.g., `my-component.component.ts`, `my-component.component.html`, `my-component.component.scss`
- Use the new `ng-template` syntax for better readability and maintainability

#### NGRX

- Use the createFeature and createReducer functions to simplify reducer creation
- Implement the facade pattern to abstract NgRx implementation details from components
- Use entity adapter for collections to standardize CRUD operations
- Leverage selectors with memoization to efficiently derive state and prevent unnecessary calculations
- Implement @ngrx/effects for handling side effects like API calls
- Use action creators with typed payloads to ensure type safety throughout the application
- Implement @ngrx/component-store for local state management in complex components
- Use @ngrx/router-store to connect the router to the store
- Leverage @ngrx/entity-data for simplified entity management
- Implement the concatLatestFrom operator for effects that need state with actions

#### ANGULAR_MATERIAL

- Create a dedicated module for Angular Material imports to keep the app module clean
- Use theme mixins to customize component styles instead of overriding CSS
- Implement OnPush change detection for performance critical components
- Leverage the CDK (Component Development Kit) for custom component behaviors
- Use Material's form field components with reactive forms for consistent validation UX
- Implement accessibility attributes and ARIA labels for interactive components
- Use the new Material 3 design system updates where available
- Leverage the Angular Material theming system for consistent branding
- Implement proper typography hierarchy using the Material typography system
- Use Angular Material's built-in a11y features like focus indicators and keyboard navigation