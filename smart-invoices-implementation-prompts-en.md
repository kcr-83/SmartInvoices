# Prompts for SmartInvoices Project Implementation

This document contains a set of prompts to help implement the SmartInvoices project from scratch. Prompts are grouped by project development stages and architecture layers.

## Table of Contents
- [Prompts for SmartInvoices Project Implementation](#prompts-for-smartinvoices-project-implementation)
  - [Table of Contents](#table-of-contents)
  - [1. Project Setup](#1-project-setup)
    - [1.1 Creating Solution Structure](#11-creating-solution-structure)
    - [1.2 Domain Layer Configuration](#12-domain-layer-configuration)
    - [1.3 Application Layer Configuration](#13-application-layer-configuration)
    - [1.4 Infrastructure Layer Configuration](#14-infrastructure-layer-configuration)
    - [1.5 API Configuration](#15-api-configuration)
  - [2. Backend Implementation](#2-backend-implementation)
    - [2.1 Domain Layer](#21-domain-layer)
    - [2.2 Application Layer](#22-application-layer)
    - [2.3 Infrastructure Layer](#23-infrastructure-layer)
    - [2.4 WebAPI Layer](#24-webapi-layer)
  - [3. Frontend Implementation](#3-frontend-implementation)
    - [3.1 Basic Configuration](#31-basic-configuration)
    - [3.2 Core Components](#32-core-components)
    - [3.3 Feature Modules](#33-feature-modules)
    - [3.4 API Integration](#34-api-integration)
  - [4. Feature Implementation](#4-feature-implementation)
    - [4.1 Invoice Management](#41-invoice-management)
    - [4.2 Line Item Change Requests](#42-line-item-change-requests)
    - [4.3 Full Refund Requests](#43-full-refund-requests)
    - [4.4 Admin Panel](#44-admin-panel)
  - [5. Testing](#5-testing)
    - [5.1 Unit Tests](#51-unit-tests)
    - [5.2 Integration Tests](#52-integration-tests)
    - [5.3 UI Tests](#53-ui-tests)
  - [6. Documentation](#6-documentation)
    - [6.1 API Documentation](#61-api-documentation)
    - [6.2 Technical Documentation](#62-technical-documentation)

## 1. Project Setup

### 1.1 Creating Solution Structure

```
Create a solution structure for the SmartInvoices project following Clean Architecture principles. I need the following projects:
- SmartInvoices.Domain (.NET Core class library)
- SmartInvoices.Application (.NET Core class library)
- SmartInvoices.Persistence (.NET Core class library)
- SmartInvoices.Infrastructure (.NET Core class library)
- SmartInvoices.Common (.NET Core class library)
- SmartInvoices.WebApi (.NET Core WebAPI project)
- SmartInvoices.Service (.NET Core Worker Service project)

Projects should have appropriate references according to Clean Architecture principles, where inner layers do not depend on outer layers.
```

### 1.2 Domain Layer Configuration

```
In the SmartInvoices.Domain project, create a basic folder structure for domain models according to Clean Architecture:
1. Entities - for main business entities
2. ValueObjects - for value objects
3. Exceptions - for domain exceptions
4. Events - for domain events
5. Interfaces - for repository interfaces
6. Enums - for enumeration types

Then create the basic enumeration types needed in the application, such as InvoiceStatus, RequestStatus, and RequestType.
```

### 1.3 Application Layer Configuration

```
Configure the SmartInvoices.Application layer, which will serve as an intermediate layer between domain and infrastructure:

1. Add a reference to the SmartInvoices.Domain project
2. Create the following folder structure:
   - Common/Mediator - for implementing a custom mediator
   - DTOs - for data transfer objects
   - Features - for CQRS functionalities, divided into subfolders by feature
   - Interfaces - for infrastructure service interfaces
   - Validators - for validation logic
   - Behaviors - for mediator pipeline behaviors

3. Implement a simple version of the mediator pattern for CQRS (without using the paid MediatR library)
```

### 1.4 Infrastructure Layer Configuration

```
Configure the infrastructure layers SmartInvoices.Persistence and SmartInvoices.Infrastructure:

1. In the SmartInvoices.Persistence project:
   - Add references to SmartInvoices.Domain and SmartInvoices.Application
   - Implement DbContext for Entity Framework Core
   - Create base configuration for entity mapping
   - Create folder structure for repositories and entity configurations
   - Prepare DependencyInjection class for service registration

2. In the SmartInvoices.Infrastructure project:
   - Add references to SmartInvoices.Domain and SmartInvoices.Application
   - Create structure for external service implementations
   - Prepare DependencyInjection class for service registration
```

### 1.5 API Configuration

```
Configure the SmartInvoices.WebApi project:

1. Add references to all necessary projects (Domain, Application, Persistence, Infrastructure, Common)
2. Configure Program.cs with appropriate service configuration and middleware:
   - Add controller support
   - Configure Swagger
   - Configure CORS
   - Add exception handling
   - Integrate dependencies from other layers using their DependencyInjection extension methods
3. Create a basic folder structure for controllers
4. Implement a basic API response model (ApiResponse)
```

## 2. Backend Implementation

### 2.1 Domain Layer

```
Implement basic domain entities for the SmartInvoices system:

1. Invoice:
   - Properties: Id, Number, IssueDate, DueDate, Status, TotalAmount, Tax, Customer, Items
   - Methods: CalculateTotal, AddItem, RemoveItem

2. InvoiceItem:
   - Properties: Id, InvoiceId, Description, Quantity, UnitPrice, TotalPrice, Tax
   - Methods: CalculatePrice

3. ChangeRequest:
   - Properties: Id, RequestDate, Status, UserId, InvoiceId, ItemsToChange, Reason, Comments
   - Methods: Approve, Reject, AddItem, RemoveItem

4. RefundRequest:
   - Properties: Id, RequestDate, Status, UserId, InvoiceId, Reason, Documents, Comments
   - Methods: Approve, Reject, AddDocument

5. User:
   - Properties: Id, Email, FirstName, LastName, Role, IsActive
   - Methods: Activate, Deactivate, ChangeRole

Entities should implement business logic according to DDD principles, ensuring data consistency and encapsulation.
```

```
Implement repository interfaces for basic entities in the SmartInvoices.Domain project:

1. IInvoiceRepository:
   - GetAllAsync
   - GetByIdAsync
   - GetByNumberAsync
   - GetWithItemsAsync
   - AddAsync
   - UpdateAsync
   - DeleteAsync

2. IRequestRepository (generic for change and refund requests):
   - GetAllAsync
   - GetByIdAsync
   - GetByUserIdAsync
   - GetByInvoiceIdAsync
   - GetPendingAsync
   - AddAsync
   - UpdateAsync

3. IUserRepository:
   - GetAllAsync
   - GetByIdAsync
   - GetByEmailAsync
   - AddAsync
   - UpdateAsync
   - DeleteAsync

Interfaces should contain typical repository methods with appropriate parameters and return types.
```

### 2.2 Application Layer

```
Implement DTOs (Data Transfer Objects) for main entities in the SmartInvoices.Application project:

1. InvoiceDto - DTO for invoice
2. InvoiceItemDto - DTO for invoice line item
3. ChangeRequestDto - DTO for change request
4. RefundRequestDto - DTO for refund request
5. UserDto - DTO for user

DTOs should contain only fields needed for the user interface, and mapping methods to/from domain entities (instead of using the paid AutoMapper).
```

```
Implement the basic CQRS structure for invoice-related functionalities in the SmartInvoices.Application project:

1. Queries:
   - GetInvoicesQuery and GetInvoicesQueryHandler - for retrieving a list of invoices with filtering
   - GetInvoiceDetailQuery and GetInvoiceDetailQueryHandler - for retrieving invoice details

2. Commands:
   - CreateInvoiceCommand and CreateInvoiceCommandHandler - for creating a new invoice
   - UpdateInvoiceCommand and UpdateInvoiceCommandHandler - for updating an invoice

Use your own mediator implementation instead of the MediatR library. Also implement command validation using FluentValidation.
```

### 2.3 Infrastructure Layer

```
Implement the persistence layer in the SmartInvoices.Persistence project:

1. SmartInvoicesDbContext - main context class for Entity Framework Core
2. Entity configurations (InvoiceConfiguration, InvoiceItemConfiguration, etc.)
3. Implementations of repositories defined in the domain layer
4. Initial migrations

Remember to properly configure relationships between entities, primary and foreign keys, and indexes.
```

```
Implement infrastructure services in the SmartInvoices.Infrastructure project:

1. EmailService - for sending email notifications
2. PdfService - for generating invoices in PDF format
3. FileStorage - for storing attachments and documents
4. JwtHandler - for handling authentication tokens
5. IdentityService - for user identity management

Services should implement interfaces defined in the application layer.
```

### 2.4 WebAPI Layer

```
Implement API controllers in the SmartInvoices.WebApi project:

1. InvoicesController - for invoice management
   - GET /api/invoices - get list of invoices with filtering
   - GET /api/invoices/{id} - get invoice details
   - POST /api/invoices - create new invoice
   - PUT /api/invoices/{id} - update invoice
   - DELETE /api/invoices/{id} - delete invoice
   - GET /api/invoices/{id}/pdf - generate PDF

2. RequestsController - for request management
   - Endpoints for change requests
   - Endpoints for refund requests

3. UsersController - for user management
   - Endpoints for user administration

4. AuthController - for authentication
   - Endpoints for login and registration

Controllers should be as thin as possible, delegating business logic to the application layer through the mediator.
```

## 3. Frontend Implementation

### 3.1 Basic Configuration

```
Create a new Angular project for the SmartInvoices application:

1. Create a new Angular project using Angular CLI:
   ng new smart-invoices --routing --style=scss

2. Add necessary libraries:
   - Angular Material for UI components
   - NgRx for application state management
   - RxJS for reactive programming

3. Configure the project structure according to Angular Style Guide:
   - core/ - for core services and models
   - shared/ - for shared components
   - features/ - for feature modules
   - admin/ - for administrative functions

4. Configure routing for main application sections
```

### 3.2 Core Components

```
Implement basic models and services in the core/ folder:

1. Models:
   - invoice.model.ts - interface for invoice
   - line-item.model.ts - interface for invoice line item
   - change-request.model.ts - interface for change request
   - refund-request.model.ts - interface for refund request
   - user.model.ts - interface for user

2. Services:
   - auth.service.ts - for authentication
   - invoice.service.ts - for API communication for invoices
   - request.service.ts - for API communication for requests
   - user.service.ts - for API communication for users

3. Guards:
   - auth.guard.ts - for protecting routes requiring authentication
   - admin.guard.ts - for protecting administrative routes
```

### 3.3 Feature Modules

```
Implement main components for invoice management:

1. InvoiceListComponent:
   - Displaying invoice list with filtering and sorting
   - Pagination for large data sets
   - Options: view details, export to PDF

2. InvoiceDetailComponent:
   - Displaying invoice details
   - List of invoice items
   - Action buttons: submit change request, submit refund request

3. InvoiceFilterComponent:
   - Filter forms for various criteria
   - Saving filter preferences
```

```
Implement components for handling requests:

1. ChangeRequestFormComponent:
   - Form for selecting items to change
   - Field for change description and justification
   - Attachment handling

2. RefundRequestFormComponent:
   - Form for refund request
   - Field for refund reason
   - Attachment handling for supporting documents

3. RequestListComponent:
   - List of submitted requests with filtering
   - Request status indicators
   - Request details viewing capability
```

### 3.4 API Integration

```
Implement API integration for invoice service (invoice.service.ts):

1. Data retrieval methods:
   - getInvoices(filters) - retrieving invoice list with filtering
   - getInvoiceById(id) - retrieving invoice details
   - downloadInvoicePdf(id) - downloading invoice in PDF format

2. Error handling:
   - Implementation of interceptor for HTTP error handling
   - Displaying appropriate messages to the user

3. Data transformation:
   - Mapping API responses to frontend models
   - Formatting dates and monetary values
```

## 4. Feature Implementation

### 4.1 Invoice Management

```
Implement invoice browsing functionality in the application:

1. In the backend layer:
   - Complete query and handler for GetInvoicesQuery
   - Filtering invoices by date, amount, and status
   - Pagination of results
   - Sorting by different columns

2. In the frontend layer:
   - Invoice list component with table
   - Pagination and sorting mechanism
   - Filter components with validation
   - Storing user filter settings
```

```
Implement invoice detail functionality and PDF export:

1. In the backend layer:
   - Query and handler for GetInvoiceDetailQuery
   - Service for generating PDF from invoice (PdfService)
   - Endpoint for downloading invoice in PDF format

2. In the frontend layer:
   - Invoice detail component with sections for general data and items
   - PDF export button
   - Displaying summaries and totals
   - Action buttons for submitting requests
```

### 4.2 Line Item Change Requests

```
Implement functionality for line item change requests:

1. In the backend layer:
   - Command and handler for CreateChangeRequestCommand
   - Request validation (checking if invoice exists, if items are valid)
   - Business logic for request handling
   - Email notifications about new request

2. In the frontend layer:
   - Form with item selection for change
   - Interface for specifying change type (quantity, price, description)
   - Justification field
   - Form validation and error messages
```

```
Implement functionality for tracking change request status:

1. In the backend layer:
   - Query and handler for GetChangeRequestsQuery
   - Query and handler for GetChangeRequestDetailQuery
   - Filtering requests by status and date

2. In the frontend layer:
   - List of requests with status information
   - Request detail component
   - Visual status indicators (e.g., color labels)
   - Notifications about request status changes
```

### 4.3 Full Refund Requests

```
Implement functionality for full refund requests:

1. In the backend layer:
   - Command and handler for CreateRefundRequestCommand
   - Request validation (checking if invoice is eligible for refund)
   - Business logic for request handling
   - Handling attachments and documentation

2. In the frontend layer:
   - Refund request form
   - Document upload interface
   - Justification field
   - Form validation and error messages
```

```
Implement functionality for tracking refund request status:

1. In the backend layer:
   - Query and handler for GetRefundRequestsQuery
   - Query and handler for GetRefundRequestDetailQuery
   - Retrieving attached documents

2. In the frontend layer:
   - List of refund requests with status information
   - Refund request detail component
   - Displaying attachments
   - Notifications about request status changes
```

### 4.4 Admin Panel

```
Implement user management panel for administrators:

1. In the backend layer:
   - Query and handler for GetUsersQuery
   - Command and handler for CreateUserCommand, UpdateUserCommand, DeactivateUserCommand
   - Permission management

2. In the frontend layer:
   - User list component with filtering and sorting
   - Forms for adding and editing users
   - Buttons for activating/deactivating accounts
   - Permission management form
```

```
Implement request management panel for administrators:

1. In the backend layer:
   - Query for retrieving all requests
   - Command and handler for ApproveRequestCommand and RejectRequestCommand
   - Adding comments to requests

2. In the frontend layer:
   - Component for list of all requests with advanced filtering
   - Interface for approving/rejecting requests
   - Comment adding form
   - Dashboard with request statistics
```

## 5. Testing

### 5.1 Unit Tests

```
Write unit tests for main domain entities:

1. Tests for Invoice:
   - Test for CalculateTotal method
   - Tests for AddItem and RemoveItem methods
   - Test for entity state validation

2. Tests for ChangeRequest:
   - Tests for Approve and Reject methods
   - Test for business logic related to item changes

3. Tests for RefundRequest:
   - Tests for Approve and Reject methods
   - Test for attachment validation
```

```
Write unit tests for command and query handlers:

1. Tests for GetInvoicesQueryHandler:
   - Test for invoice filtering
   - Test for result pagination
   - Test for result sorting

2. Tests for CreateChangeRequestCommandHandler:
   - Test for proper request creation
   - Test for validation (e.g., invalid invoice, invalid items)
   - Test for error handling
```

### 5.2 Integration Tests

```
Write integration tests for persistence layer:

1. Tests for repositories:
   - Test for saving and reading entities
   - Test for filtering and sorting
   - Test for relationships between entities

2. Tests for DbContext:
   - Test for relationship configuration
   - Test for property mapping
   - Test for cascade deletion
```

```
Write integration tests for API controllers:

1. Tests for InvoicesController:
   - Test for retrieving invoice list
   - Test for retrieving invoice details
   - Test for creating and updating invoices

2. Tests for RequestsController:
   - Test for submitting requests
   - Test for retrieving request status
   - Test for updating request status
```

### 5.3 UI Tests

```
Write tests for Angular components:

1. Tests for InvoiceListComponent:
   - Test for rendering invoice list
   - Test for filter functionality
   - Test for pagination and sorting

2. Tests for request forms:
   - Test for form validation
   - Test for form submission
   - Test for error handling
```

## 6. Documentation

### 6.1 API Documentation

```
Configure and expand Swagger API documentation:

1. Add detailed descriptions for all endpoints
2. Define example responses and requests
3. Group endpoints by functionality
4. Add authorization to Swagger documentation
5. Version the API in documentation
```

### 6.2 Technical Documentation

```
Create ADR (Architecture Decision Record) documentation for key decisions:

1. Choice of Clean Architecture
2. Implementation of custom mediator instead of MediatR
3. Error and exception handling strategy
4. Input data validation approach
5. Testing strategy
```

```
Create documentation for the development team:

1. Installation and environment configuration instructions
2. Project structure and convention description
3. Process for adding new features
4. Testing and deployment procedures
5. Project-specific best practices
```
