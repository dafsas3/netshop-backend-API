# NetShopAPI

Backend REST API for an online shop built with ASP.NET Core.

> Project is under active development (Work in Progress)

## Tech Stack
- ASP.NET Core (.NET 8)
- Entity Framework Core
- MySQL
- JWT Authentication
- FluentValidation
- REST API
- Git

## Architecture
- Layered architecture (Controllers / Services / Data / DTO)
- Thin controllers
- Business logic isolated in services
- Result<T> pattern for consistent API responses
- Centralized exception handling middleware

## Implemented Features
- User registration & authentication (JWT)
- Role-based authorization
- Product & category management
- Shopping cart functionality
- Order creation
- Order status workflow with validation rules
- Data validation (FluentValidation)
- Proper HTTP status codes
- Supply management module (partially implemented)

## Order Workflow
Order statuses are managed using enums and centralized workflow rules:
- Created
- Paid
- Processing
- Shipped
- Completed
- Cancelled

Invalid status transitions are blocked at service level.

## Database
- MySQL
- Code First migrations (EF Core)

## API Documentation
- Swagger (OpenAPI)

## Project Status
The project is not fully completed yet.
Planned improvements:
- Transactions for order creation
- Product stock validation
- Unit testing
- Pagination and filtering
- Access policies

> Note:
> Recipe management service was implemented temporarily for a specific purpose and will be removed in future versions.

## Author
Backend Developer (Junior / Junior+)
