# Nest.js Unit Test Generator

Your goal is to generate comprehensive unit tests for Nest.js application components.

Ask for the controller/service/provider name and purpose if not provided.

## Requirements for the tests:

### Test Structure
- Create appropriate describe/it blocks with clear, descriptive names
- Follow the Arrange-Act-Assert pattern in each test
- Include both positive and negative test cases
- Test one behavior per test case

### Controller Testing Best Practices
- Use Nest.js Testing Module for configuring the testing environment
- Test controller initialization, routes, and methods
- Mock services and other dependencies
- Test request validation and error handling
- Verify proper response status codes and body formats
- Test route guards and interceptors when applicable

### Service Testing Best Practices
- Test service initialization and methods
- Mock external dependencies using Jest mock functions
- Mock database repositories/models for isolation
- Test error handling scenarios
- Test business logic thoroughly with various inputs
- Use Jest's timer mocks for testing time-dependent code

### Provider/Guard/Interceptor Testing
- Focus on the specific functionality they provide
- Mock dependencies and context objects
- Test different scenarios and edge cases
- Verify expected behavior under various conditions

### Mocking Best Practices
- Use Jest's mock functions (jest.fn(), jest.spyOn())
- Create complete mock objects for complex dependencies
- Implement mock providers in Testing Module configuration
- Use mockResolvedValue/mockRejectedValue for async operations
- Reset mocks between tests when needed

### Test-Driven Development
- Write failing tests before implementing functionality
- Define expected behavior through assertions first
- Focus on testing public interfaces, not implementation details
- Keep tests independent and isolated

### TypeScript Type Safety
- Use proper typing for all test variables, mocks, and test data
- Create dedicated interfaces/types for test data when needed
- Use typing to ensure complete coverage of edge cases
- Leverage TypeScript to validate expected return types

### Code Quality
- DRY principle: extract common setup into beforeEach/beforeAll
- Avoid test interdependence
- Use meaningful test data that clearly demonstrates intent
- Include comments explaining complex test scenarios
- Follow Jest best practices for clean, maintainable tests

### Example structure:

```typescript
import { Test, TestingModule } from '@nestjs/testing';
import { ExampleService } from './example.service';
import { ExampleRepository } from './example.repository';

describe('ExampleService', () => {
  let service: ExampleService;
  let repositoryMock: jest.Mocked<ExampleRepository>;

  beforeEach(async () => {
    const mockRepository = {
      findOne: jest.fn(),
      find: jest.fn(),
      create: jest.fn(),
      save: jest.fn(),
      update: jest.fn(),
      delete: jest.fn(),
    };

    const module: TestingModule = await Test.createTestingModule({
      providers: [
        ExampleService,
        {
          provide: ExampleRepository,
          useValue: mockRepository,
        },
      ],
    }).compile();

    service = module.get<ExampleService>(ExampleService);
    repositoryMock = module.get(ExampleRepository) as jest.Mocked<ExampleRepository>;
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('findById', () => {
    it('should return an item when found', async () => {
      // Arrange
      const mockItem = { id: 1, name: 'Test' };
      repositoryMock.findOne.mockResolvedValue(mockItem);
      
      // Act
      const result = await service.findById(1);
      
      // Assert
      expect(result).toEqual(mockItem);
      expect(repositoryMock.findOne).toHaveBeenCalledWith({ where: { id: 1 } });
    });

    it('should throw NotFoundException when item not found', async () => {
      // Arrange
      repositoryMock.findOne.mockResolvedValue(null);
      
      // Act & Assert
      await expect(service.findById(999)).rejects.toThrow('Item not found');
      expect(repositoryMock.findOne).toHaveBeenCalledWith({ where: { id: 999 } });
    });
  });

  describe('create', () => {
    it('should create and return a new item', async () => {
      // Arrange
      const createDto = { name: 'New Item' };
      const newItem = { id: 1, ...createDto };
      
      repositoryMock.create.mockReturnValue(newItem as any);
      repositoryMock.save.mockResolvedValue(newItem as any);
      
      // Act
      const result = await service.create(createDto);
      
      // Assert
      expect(result).toEqual(newItem);
      expect(repositoryMock.create).toHaveBeenCalledWith(createDto);
      expect(repositoryMock.save).toHaveBeenCalledWith(newItem);
    });
  });
});
```