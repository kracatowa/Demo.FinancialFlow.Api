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
- **Demo.FinancialFlow.Domain** : Contains the core business logic, domain entities, value objects, and domain services.
- **Demo.FinancialFlow.Infrastructure**: Connect to the infrastructure layer, including data access, external services, and other technical implementations.

## API Endpoints

### Health Check
`GET /api/financialflow/health`
- Returns: `true` if the service is healthy.
- Only for testing purposes

### Upload Financial Flow File
`POST /api/financialflow/upload`
- Body: `multipart/form-data` with a file field named `File`
- Accepts: `.csv` files up to 10MB
- Returns: `true` on success, error message otherwise

## Cloud Agnostic

### Running the Project with Dapr

To start the API with Dapr sidecar and load the required components, use the following command from the project root:

dapr run --app-id financialflowapi --app-port 5000 --resources-path ./components -- dotnet run --project Demo.FinancialFlow.Api/Demo.FinancialFlow.Api.csproj

This will:
- Start the Dapr sidecar with your configured components (in the `./components` directory)
- Launch the API on port 5000
- Register the app with Dapr as `financialflowapi`

Make sure you have Dapr installed and initialized before running this command.