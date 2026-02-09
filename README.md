# NetShopAPI

Backend REST API for an online shop built with ASP.NET Core (.NET 8).

🚧 Project is under active development (Work in Progress)

---

## Overview
NetShopAPI is a backend-focused project designed not only to implement typical e-commerce features, but also to demonstrate production backend engineering practices, including:
- Data consistency strategies
- Database-level data integrity
- Graceful error handling
- Clean architecture separation
- Performance-aware database design
- Concurrency-safe operations

---

## Tech Stack
- ASP.NET Core (.NET 8)
- Entity Framework Core
- MySQL
- SQLite (tests)
- JWT Authentication
- FluentValidation
- Swagger (OpenAPI)
- xUnit (Unit testing)
- Unit-testing
- Integration testing

---

## Architecture
Core Architecture Style
- CQRS-lite (Commands / Queries, Handlers / Repositories)
- Layered architecture
- Thin Controllers
- Business logic isolated in handlers and services
- Repository + Unit Of Work pattern
- Result pattern for predictable API responses

---

## Backend Design Highlights
- CQRS-lite separation of write and read operations
- Centralized validation via FluentValidation
- Database-level data integrity using unique indexes
- Graceful handling of race conditions (duplicate key → 409 Conflict)
- Enum storage optimized to tinyint (byte enum) for DB performance
- Result pattern for consistent API responses
- Centralized exception handling middleware

---

## Data Integrity & Concurrency
- Data consistency is enforced at the database level using unique constraints and indexed fields.
- The database is considered the final authority for data validity, while the application layer provides user-friendly error mapping and validation.
- Key strategies implemented:
- Unique constraints for critical identity fields (Email, Phone, Nickname)
- Graceful handling of duplicate key conflicts (mapped to HTTP 409 Conflict)
- Input normalization before persistence (trimmed values, normalized email casing)
- Protection against race conditions during concurrent registration attempts

---

## Performance Considerations
- Indexed frequently queried fields
- Optimized enum storage (byte → tinyint in DB)
- Reduced index size for faster queries
- Bulk operations support
- Optimized query patterns (AsNoTracking for read queries)

---

## Security
- JWT authentication
- Password hashing via ASP.NET Core Identity PasswordHasher
- Validation layer before persistence
- Controlled error exposure via centralized middleware

---

## Implemented Features
### Core business functionality:
- User registration and JWT-based authentication
- Role-based authorization (RBAC)
- Product and category management
- Shopping cart lifecycle management
- Order creation and processing pipeline
- Order workflow validation with protected status transitions

### Backend engineering practices demonstrated:
- Input validation using FluentValidation with separation of validation layer
- Consistent API responses via Result pattern
- Correct HTTP status mapping for business and infrastructure errors
- Centralized exception handling middleware
- Supply management module (in progress)

---

## Order Workflow
- Order statuses are implemented using enums and centralized workflow rules.

Statuses:
- Created
- Paid
- Processing
- Shipped
- Completed
- Cancelled

- Invalid transitions are blocked at business logic level.

---

## Database
- MySQL (Primary DB)
- SQLite (Testing)
- Code First migrations (EF Core)
- Unique constraints for critical user fields
- Optimized enum storage (tinyint)

---

## Testing
- Unit tests for business logic
- Integration tests for API endpoints
- SQLite used for fast isolated testing

---

## API Documentation
- Swagger / OpenAPI available in development mode.

---

## Why This Project
- This project focuses not only on CRUD operations, but on applying real backend engineering practices:
- Predictable error handling
- Database consistency guarantees
- Scalable architecture design
- Separation of business logic
- Production-style validation and error mapping
---
## Project Status

---

🚧 Active development
- New modules and improvements are continuously being added.
