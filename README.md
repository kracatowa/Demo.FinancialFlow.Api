# Demo.FinancialFlow.Api
Backend for a treasury analysis SaaS

## Architecture

This project’s structure is inspired by Clean Architecture and Domain-Driven Design (DDD) principles.  
It is organized to promote separation of concerns, maintainability, and scalability for .NET backend solutions.

A closely related reference project by Microsoft is [eShopOnContainers](https://github.com/dotnet/eShop), which demonstrates similar architectural patterns and best practices.

For more details and documentation, see:  
https://github.com/dotnet/eShop

## Persistence
A relational SQL database is used for data persistence.

## Project Structure

- **Demo.FinancialFlow.Api**: The main entry point of the application, containing controllers and API-related configurations.
- **Demo.FinancialFlow.Domain : Contains the core business logic, domain entities, value objects, and domain services.
- **Demo.FinancialFlow.Infrastructure**: Connect to the infrastructure layer, including data access, external services, and other technical implementations.
