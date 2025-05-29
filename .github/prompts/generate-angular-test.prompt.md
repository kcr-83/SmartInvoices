# Angular Unit Test Generator

Your goal is to generate comprehensive unit tests for Angular application components.

Ask for the component/service/function name and purpose if not provided.

## Requirements for the tests:

### Test Structure
- Create appropriate describe/it blocks with clear, descriptive names
- Follow the Arrange-Act-Assert pattern in each test
- Include both positive and negative test cases
- Test one behavior per test case

### Component Testing Best Practices
- Use TestBed for configuring the testing module
- Test component initialization, input/output properties, and methods
- Mock child components when needed using NO_ERRORS_SCHEMA or stub components
- Verify DOM changes with fixture.debugElement or nativeElement
- Use ComponentFixture's detectChanges() to trigger change detection
- Implement component isolation tests (without TestBed) where appropriate

### Service Testing Best Practices
- Test service initialization and methods
- Mock HttpClient using HttpClientTestingModule
- Use jasmine spies for dependencies
- Test error handling scenarios
- Use fakeAsync/tick for testing async operations

### Function/Pipe Testing
- Focus on input/output scenarios
- Test edge cases and error conditions
- For pure pipes, test transformation logic without Angular dependencies

### Mocking Best Practices
- Create mock services with jasmine.createSpyObj or manual mocks
- Implement mock providers in TestBed configuration
- Use jasmine.Spy methods (and.returnValue, and.callFake, etc.)
- Reset spies between tests when needed

### Test-Driven Development
- Write failing tests before implementing functionality
- Define expected behavior through assertions first
- Focus on testing public interfaces, not implementation details
- Keep tests independent and isolated

### TypeScript Type Safety
- Use proper typing for all test variables, mocks, and test data
- Create dedicated interfaces/types for test data when needed
- Use typing to ensure complete coverage of enum values and edge cases

### Code Quality
- DRY principle: extract common setup into beforeEach
- Avoid test interdependence
- Use meaningful test data that clearly demonstrates intent
- Include comments explaining complex test scenarios

### Example structure:

```typescript
describe('ComponentName', () => {
  let component: ComponentName;
  let fixture: ComponentFixture<ComponentName>;
  let dependencyMock: jasmine.SpyObj<DependencyService>;

  beforeEach(() => {
    dependencyMock = jasmine.createSpyObj('DependencyService', ['method1', 'method2']);
    
    TestBed.configureTestingModule({
      declarations: [ComponentName],
      providers: [
        { provide: DependencyService, useValue: dependencyMock }
      ],
      imports: [CommonModule]
    }).compileComponents();
    
    fixture = TestBed.createComponent(ComponentName);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should handle specific behavior', () => {
    // Arrange
    dependencyMock.method1.and.returnValue('test');
    
    // Act
    component.doSomething();
    fixture.detectChanges();
    
    // Assert
    expect(component.result).toBe('expected value');
    expect(dependencyMock.method1).toHaveBeenCalledWith('test parameter');
  });
});
```