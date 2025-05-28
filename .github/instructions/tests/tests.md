# Testing Instructions for GitHub Copilot

## Backend Testing (.NET Core Web API)

### Testing Frameworks & Libraries
- **xUnit**: Primary testing framework
- **Shouldly**: Assertion library for more readable assertions
- **Moq**: For mocking dependencies in unit tests
- **Microsoft.AspNetCore.Mvc.Testing**: For integration tests

### Unit Test Structure
When generating unit tests, follow this pattern:

```csharp
using System;
using Xunit;
using Shouldly;
using Moq;

namespace YourProject.Tests.Unit
{
    public class ClassNameTests
    {
        // Arrange, Act, Assert pattern for each test
        [Fact]
        public void MethodName_Scenario_ExpectedBehavior()
        {
            // Arrange
            var dependencies = new Mock<IDependency>();
            dependencies.Setup(d => d.Method()).Returns(expectedValue);
            var sut = new SystemUnderTest(dependencies.Object);

            // Act
            var result = sut.MethodUnderTest(parameter);

            // Assert
            result.ShouldBe(expectedValue);
            dependencies.Verify(d => d.Method(), Times.Once());
        }
        
        // Use [Theory] and [InlineData] for parameterized tests
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(5, 5, 10)]
        public void Add_WhenCalled_ReturnsSumOfInputs(int a, int b, int expected)
        {
            // Arrange
            var calculator = new Calculator();
            
            // Act
            var result = calculator.Add(a, b);
            
            // Assert
            result.ShouldBe(expected);
        }
    }
}
```

### Test Coverage Guidelines
- Controllers: Test each action with different inputs and response types
- Services: Test business logic, especially edge cases
- Repositories: Mock database context and test CRUD operations
- Middleware: Test with mock request/response pipelines
- Validators: Test with valid and invalid inputs

### Integration Test Structure

```csharp
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Shouldly;

namespace YourProject.Tests.Integration
{
    public class ApiEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApiEndpointTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetEndpoint_ReturnsSuccessStatusCode()
        {
            // Arrange
            // Already arranged with test client

            // Act
            var response = await _client.GetAsync("/api/resource");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<ExpectedType>();
            content.ShouldNotBeNull();
            content.Property.ShouldBe(expectedValue);
        }
    }
}
```

### Database Test Structure
For testing database operations, use an in-memory database:

```csharp
using Microsoft.EntityFrameworkCore;
using Xunit;
using Shouldly;

namespace YourProject.Tests.Database
{
    public class RepositoryTests
    {
        [Fact]
        public async Task GetById_ExistingId_ReturnsEntity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
                
            using (var context = new AppDbContext(options))
            {
                context.Entities.Add(new Entity { Id = 1, Name = "Test" });
                await context.SaveChangesAsync();
            }
            
            // Act
            using (var context = new AppDbContext(options))
            {
                var repository = new EntityRepository(context);
                var result = await repository.GetByIdAsync(1);
                
                // Assert
                result.ShouldNotBeNull();
                result.Name.ShouldBe("Test");
            }
        }
    }
}
```

## Frontend Testing (Angular)

### Testing Frameworks & Libraries
- **Jasmine**: Primary testing framework
- **Karma**: Test runner
- **Angular Testing Utilities**: TestBed, ComponentFixture, etc.
- **Jest**: Optional alternative to Jasmine/Karma (if preferred)

### Component Testing Structure

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { YourComponent } from './your.component';
import { SharedModule } from '../shared/shared.module';
import { provideMockStore } from '@ngrx/store/testing';

describe('YourComponent', () => {
  let component: YourComponent;
  let fixture: ComponentFixture<YourComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SharedModule],
      declarations: [YourComponent],
      providers: [
        // Mock services/store if needed
        provideMockStore({ initialState: {} })
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(YourComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should display the title', () => {
    const titleElement = fixture.debugElement.query(By.css('h1'));
    expect(titleElement.nativeElement.textContent).toContain('Expected Title');
  });

  it('should call method when button clicked', () => {
    // Arrange
    spyOn(component, 'handleClick');
    const button = fixture.debugElement.query(By.css('button'));
    
    // Act
    button.triggerEventHandler('click', null);
    
    // Assert
    expect(component.handleClick).toHaveBeenCalled();
  });
});
```

### Service Testing Structure

```typescript
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { YourService } from './your.service';

describe('YourService', () => {
  let service: YourService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [YourService]
    });
    service = TestBed.inject(YourService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should retrieve data from the API', () => {
    const mockData = [{ id: 1, name: 'Test' }];
    
    service.getData().subscribe(data => {
      expect(data).toEqual(mockData);
    });
    
    const req = httpMock.expectOne(`${service.apiUrl}/data`);
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });
});
```

### Pipe and Directive Testing

```typescript
import { CapitalizePipe } from './capitalize.pipe';

describe('CapitalizePipe', () => {
  let pipe: CapitalizePipe;

  beforeEach(() => {
    pipe = new CapitalizePipe();
  });

  it('transforms "abc" to "Abc"', () => {
    expect(pipe.transform('abc')).toBe('Abc');
  });

  it('does not transform already capitalized strings', () => {
    expect(pipe.transform('Abc')).toBe('Abc');
  });
});
```

### Router Testing Structure

```typescript
import { Location } from '@angular/common';
import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { HomeComponent } from './home.component';
import { DetailComponent } from './detail.component';

describe('Router: App', () => {
  let location: Location;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
          { path: 'home', component: HomeComponent },
          { path: 'detail/:id', component: DetailComponent }
        ])
      ],
      declarations: [HomeComponent, DetailComponent]
    });

    router = TestBed.inject(Router);
    location = TestBed.inject(Location);
    router.initialNavigation();
  });

  it('navigate to "home" takes you to /home', fakeAsync(() => {
    router.navigate(['/home']);
    tick();
    expect(location.path()).toBe('/home');
  }));
});
```

### Integration Testing with MockBackend
For testing components with API dependencies:

```typescript
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { UsersComponent } from './users.component';
import { UserService } from './user.service';
import { of } from 'rxjs';

describe('UsersComponent', () => {
  let component: UsersComponent;
  let fixture: ComponentFixture<UsersComponent>;
  let userService: UserService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UsersComponent],
      imports: [HttpClientTestingModule],
      providers: [UserService]
    }).compileComponents();

    fixture = TestBed.createComponent(UsersComponent);
    component = fixture.componentInstance;
    userService = TestBed.inject(UserService);
    
    // Mock service response
    spyOn(userService, 'getUsers').and.returnValue(of([
      { id: 1, name: 'User 1' },
      { id: 2, name: 'User 2' }
    ]));
    
    fixture.detectChanges();
  });

  it('should retrieve and display users', () => {
    expect(userService.getUsers).toHaveBeenCalled();
    expect(component.users.length).toBe(2);
    
    // Check DOM rendering
    const userElements = fixture.nativeElement.querySelectorAll('.user-item');
    expect(userElements.length).toBe(2);
    expect(userElements[0].textContent).toContain('User 1');
  });
});
```

### Testing Best Practices
1. **Test isolation**: Each test should be independent
2. **Arrange-Act-Assert**: Follow this pattern for clarity
3. **Mock external dependencies**: Use jasmine spies or TestBed providers
4. **Focus on behavior, not implementation**: Test what components do, not how they do it
5. **Use TestBed properly**: Configure once in beforeEach
6. **Test DOM interactions**: Use debugElement and By.css queries
7. **Async testing**: Use fakeAsync/tick or async/await for asynchronous operations

## General Guidelines for Copilot

When generating tests with Copilot, please:

1. **Follow naming conventions**: [ClassName]Tests for class tests
2. **Use descriptive test names**: MethodName_Scenario_ExpectedBehavior format
3. **Prioritize edge cases**: Test boundary conditions and error paths
4. **Follow project structure**: Place tests in corresponding test folders
5. **Generate appropriate mocks**: Create mock objects for dependencies
6. **Use test data builders**: For complex test data setup
7. **Keep tests simple**: One assertion per test when possible
8. **Include comments**: Explain the purpose of complex test scenarios
9. **Generate parameterized tests**: For similar test cases with different inputs
10. **Consider test coverage**: Aim for comprehensive coverage of public methods