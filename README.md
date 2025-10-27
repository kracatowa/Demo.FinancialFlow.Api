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

### Upload Financial Flow File (Start)
`POST /api/financialflow/upload/start`
- Body: `multipart/form-data` with fields:
  - `File` (required): The CSV file to upload (max 10MB)
  - `UserId` (required): The user identifier
- Accepts: `.csv` files up to 10MB
- Returns: `true` on success, error message otherwise

### Process Financial Flow File
`POST /api/financialflow/upload/process`
- Body: JSON
  - `subject` (required): The file reference in the format `{guid}.csv` or path ending with `{guid}.csv`
- Example: { "subject": "folder/subfolder/a7aaf4e7-92d7-44d5-b4dc-1bb37a27fa28.csv" }
- Returns: `true` on success, error message otherwise

### Query Financial Flows
`GET /api/financialflow`
- Query parameters (all optional except paging):
  - `PageNumber` (default: 1)
  - `PageSize` (default: 10)
  - `MinAmount`, `MaxAmount`
  - `FromDate`, `ToDate`
  - `Description`
  - `FlowType`
  - `Subsidiary`
- Returns: { "totalCount": <int>, "items": [ ... ] }

## Cloud Agnostic

### Running the Project with Dapr

To start the API with Dapr sidecar and load the required components, use the following command from the project root:

dapr run --app-id financialflowapi --app-port 5000 --resources-path ./components -- dotnet run --project Demo.FinancialFlow.Api/Demo.FinancialFlow.Api.csproj

This will:
- Start the Dapr sidecar with your configured components (in the `./components` directory)
- Launch the API on port 5000
- Register the app with Dapr as `financialflowapi`

Make sure you have Dapr installed and initialized before running this command.