# Nest.js E2E Test Generator

Your goal is to generate comprehensive end-to-end (e2e) tests for Nest.js API endpoints.

Ask for the controller/route and its purpose if not provided.

## File Structure and Location

- Tests should be created in `apps/backend-nest-e2e/src/app` folder
- Follow the same folder structure as the application code in `apps/backend-nest/src/app`
- Use a consistent naming convention: `feature.controller.e2e-spec.ts` 

## Requirements for the tests:

### Test Structure
- Use `describe`/`it` blocks with clear, descriptive names
- Follow the Arrange-Act-Assert pattern in each test
- Group related tests using nested `describe` blocks by endpoint or functionality
- Include both positive and negative test cases
- Test one behavior per test case
- Add explanatory comments/JSDoc for the test suite

### E2E Testing Best Practices
- Use Nest.js TestingModule to bootstrap the application
- Set up proper database connections using `getDataSourceToken`
- Use Supertest to perform HTTP requests against the app
- Apply global pipes (e.g., ValidationPipe) as in production
- Implement proper test initialization and teardown with beforeAll/afterAll
- Test HTTP status codes, response bodies, and headers
- Validate request payloads and error responses
- Test authentication/authorization scenarios with different user roles
- Cover edge cases and invalid input scenarios

### Authentication and Authorization Testing
- Create helper functions to generate JWT tokens for different user roles
- Test endpoints with and without authentication
- Verify proper authorization checks for different user roles
- Test access control (users only seeing their resources)

### Database Handling
- Use the actual database for e2e tests
- Set up test data directly via repositories in beforeAll
- Use unique identifiers (timestamps, UUIDs) for test data to avoid conflicts
- Implement helper functions for data generation
- Clean up test data after tests when appropriate

### TypeScript Type Safety
- Use proper typing for all test variables
- Import entities and DTOs from the main application
- Create interfaces/types for test data when needed

### Example structure:

```typescript
// filepath: apps/backend-nest-e2e/src/app/feature/feature.controller.e2e-spec.ts
import { INestApplication, ValidationPipe } from '@nestjs/common';
import { Test, TestingModule } from '@nestjs/testing';
import { getDataSourceToken } from '@nestjs/typeorm';
import request from 'supertest';
import { DataSource, Repository } from 'typeorm';
import { AppModule } from '../../../../backend-nest/src/app/app.module';
import { JwtService } from '@nestjs/jwt';
import { FeatureEntity } from '../../../../backend-nest/src/app/feature/entities/feature.entity';
import { User, UserRole } from '../../../../backend-nest/src/app/users/entities/user.entity';

/**
 * E2E tests for FeatureController
 * Covers authentication, authorization, validation, and business rules
 */
describe('FeatureController (e2e)', () => {
  let app: INestApplication;
  let jwtService: JwtService;
  let dataSource: DataSource;
  let userRepository: Repository<User>;
  let featureRepository: Repository<FeatureEntity>;
  let customerToken: string;
  let adminToken: string;
  let testRunTimestamp: string;
  const customerUserId = 101;
  const adminUserId = 1;
  
  // Helper to generate JWT tokens for different roles
  function generateToken(userId: number, role: UserRole): string {
    return jwtService.sign({ sub: userId, role });
  }
  
  // Helper to generate unique test data
  function generateUniqueIdentifier(prefix: string): string {
    return `${prefix}-${testRunTimestamp}`;
  }

  beforeAll(async () => {
    const moduleFixture: TestingModule = await Test.createTestingModule({
      imports: [AppModule],
    }).compile();
    
    app = moduleFixture.createNestApplication();
    app.useGlobalPipes(new ValidationPipe({ whitelist: true, forbidNonWhitelisted: true }));
    await app.init();
    
    jwtService = app.get(JwtService);
    customerToken = generateToken(customerUserId, UserRole.CUSTOMER);
    adminToken = generateToken(adminUserId, UserRole.ADMIN);
    
    // Use timestamp to ensure unique data across test runs
    testRunTimestamp = new Date().getTime().toString();

    // Get the database connection and repositories
    dataSource = app.get<DataSource>(getDataSourceToken());
    userRepository = dataSource.getRepository(User);
    featureRepository = dataSource.getRepository(FeatureEntity);

    // Insert test users
    await userRepository.save([
      {
        id: customerUserId,
        username: 'customer',
        email: 'customer@example.com',
        passwordHash: 'test',
        role: UserRole.CUSTOMER,
        firstName: 'Test',
        lastName: 'Customer'
      },
      {
        id: adminUserId,
        username: 'admin',
        email: 'admin@example.com',
        passwordHash: 'test',
        role: UserRole.ADMIN,
        firstName: 'Test',
        lastName: 'Admin'
      }
    ]);
  });

  afterAll(async () => {
    await app.close();
  });

  describe('POST /feature', () => {
    it('should create a new resource as authorized user', async () => {
      const createDto = {
        name: generateUniqueIdentifier('test-feature'),
        description: 'Test description'
      };
      
      const response = await request(app.getHttpServer())
        .post('/feature')
        .set('Authorization', `Bearer ${customerToken}`)
        .send(createDto);
        
      expect(response.status).toBe(201);
      expect(response.body).toHaveProperty('id');
      expect(response.body.name).toBe(createDto.name);
    });
    
    it('should fail to create with invalid data', async () => {
      const response = await request(app.getHttpServer())
        .post('/feature')
        .set('Authorization', `Bearer ${customerToken}`)
        .send({});
        
      expect(response.status).toBe(400);
    });
    
    it('should fail without authentication', async () => {
      const response = await request(app.getHttpServer())
        .post('/feature')
        .send({ name: 'Unauthorized' });
        
      expect(response.status).toBe(401);
    });
  });

  // Additional endpoint tests for GET, PATCH, DELETE etc.
});
```

## Real-World Example from Invoices E2E Tests

Here's a more complex example showing authentication, role-based access, and business rule testing:

```typescript
describe('PATCH /invoices/:id/status', () => {
  it('should allow admin to update status', async () => {
    const response = await request(app.getHttpServer())
      .patch(`/invoices/${createdInvoiceId}/status`)
      .set('Authorization', `Bearer ${adminToken}`)
      .send({ status: InvoiceStatus.ISSUED });
    expect(response.status).toBe(200);
    expect(response.body.status).toBe(InvoiceStatus.ISSUED);
  });
  
  it('should not allow customer to update status', async () => {
    const response = await request(app.getHttpServer())
      .patch(`/invoices/${createdInvoiceId}/status`)
      .set('Authorization', `Bearer ${customerToken}`)
      .send({ status: InvoiceStatus.DRAFT });
    expect([403, 401]).toContain(response.status);
  });
}); 

describe('DELETE /invoices/:id', () => {
  it('should allow admin to delete invoice', async () => {
    // First create a new invoice for this test
    const createResponse = await request(app.getHttpServer())
      .post('/invoices')
      .set('Authorization', `Bearer ${customerToken}`)
      .send(createInvoiceDto);
      
    expect(createResponse.status).toBe(201);
    const invoiceToDeleteId = createResponse.body.id;
    
    // Then delete the invoice
    const deleteResponse = await request(app.getHttpServer())
      .delete(`/invoices/${invoiceToDeleteId}`)
      .set('Authorization', `Bearer ${adminToken}`);
    
    expect(deleteResponse.status).toBe(200);
    expect(deleteResponse.body.success).toBe(true);
  });
});
```

## API Testing Best Practices

1. **Test Isolation**: Each test should be independent and not rely on the state from previous tests
2. **Comprehensive Coverage**: Test all endpoints and HTTP methods
3. **Authentication Testing**: Verify both valid and invalid authentication scenarios
4. **Authorization Testing**: Test access control for different user roles
5. **Input Validation**: Test with valid, invalid, and edge-case inputs
6. **Error Handling**: Verify appropriate error responses and status codes
7. **Pagination and Filtering**: Test list endpoints with various query parameters
8. **Transaction Testing**: For critical operations, verify the entire transaction was completed
9. **Status Code Verification**: Always verify the correct HTTP status codes
10. **Response Structure**: Verify the response body structure and content
11. **Performance Considerations**: Add minimal performance expectations for critical endpoints
12. **Business Rules**: Verify that business logic is correctly implemented
