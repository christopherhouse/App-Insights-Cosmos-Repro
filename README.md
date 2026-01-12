# Cosmos Customer API

A .NET 8 Web API for managing customer data with Azure Cosmos DB and Application Insights telemetry.

## Features

- **CRUD Operations** for Customer entities
- **Azure Cosmos DB** integration using SDK v3
- **Application Insights** telemetry (non-OpenTelemetry configuration)
- **Swagger/OpenAPI** documentation
- **No Authentication** (as per requirements)

## Customer Model

The Customer entity includes typical ecommerce attributes:

- `customerId` - Partition key (camelCase serialization)
- `firstName`, `lastName`
- `email`, `phoneNumber`
- `address` (street, city, state, postalCode, country)
- `dateOfBirth`
- `accountCreatedDate`
- `isActive`
- `loyaltyPoints`
- `totalOrderValue`

## Prerequisites

- .NET 8 SDK
- Azure Cosmos DB account (or Cosmos DB Emulator for local development)
- Azure Application Insights resource

## Configuration

Update `appsettings.json` or `appsettings.Development.json` with your connection strings:

```json
{
  "ApplicationInsights": {
    "ConnectionString": "YOUR_APPLICATION_INSIGHTS_CONNECTION_STRING"
  },
  "CosmosDb": {
    "EndpointUri": "YOUR_COSMOS_DB_ENDPOINT_URI",
    "PrimaryKey": "YOUR_COSMOS_DB_PRIMARY_KEY",
    "DatabaseName": "CustomersDB",
    "ContainerName": "Customers"
  }
}
```

### Local Development

For local development, `appsettings.Development.json` is pre-configured with Cosmos DB Emulator defaults:
- Endpoint: `https://localhost:8081`
- Default emulator primary key

## API Endpoints

- `GET /api/customers` - Get all customers
- `GET /api/customers/{customerId}` - Get a specific customer
- `POST /api/customers` - Create a new customer
- `PUT /api/customers/{customerId}` - Update an existing customer
- `DELETE /api/customers/{customerId}` - Delete a customer

## Running the Application

```bash
cd CosmosCustomerApi
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`

## Database Setup

Before running the API, ensure your Cosmos DB has:
1. A database named `CustomersDB`
2. A container named `Customers` with partition key `/customerId`

You can create these using the Azure Portal, Azure CLI, or the Cosmos DB Emulator Data Explorer.

## Technology Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Azure Cosmos DB SDK v3** (Microsoft.Azure.Cosmos 3.44.1)
- **Application Insights SDK** (Microsoft.ApplicationInsights.AspNetCore 2.22.0)
- **Swagger/OpenAPI** for API documentation

## Project Structure

```
CosmosCustomerApi/
├── Controllers/
│   └── CustomersController.cs    # CRUD endpoints
├── Models/
│   └── Customer.cs                # Customer and Address models
├── Services/
│   ├── ICosmosDbService.cs        # Service interface
│   └── CosmosDbService.cs         # Cosmos DB implementation
├── Program.cs                      # Application configuration
└── appsettings.json               # Configuration settings
```

## Notes

- The API does not implement authentication as per requirements
- Application Insights is configured without OpenTelemetry
- Cosmos DB operations use the native SDK (no Entity Framework)
- Customer ID is used as the partition key for optimal performance
