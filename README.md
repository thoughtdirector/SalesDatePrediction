
# 📊 Sales Date Prediction – Backend API

This project implements a .NET Core Web API that powers the *Sales Date Prediction* application. Its main goal is to create sales orders and predict the date of the next order for each customer, based on historical transaction data.

## 🧱 Project Structure

The solution follows a layered architecture and adheres to SOLID principles:

- **SalesDatePrediction.API** – API layer exposing REST endpoints.
- **SalesDatePrediction.Core** – Domain layer containing entities, interfaces, and application layer containing DTOs.
- **SalesDatePrediction.Infrastructure** – Data access layer with repository implementations.
- **SalesDatePrediction.Tests** – Unit test project for core components.

## ✅ Prerequisites

Before getting started, ensure you have the following:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later
- A local or remote instance of **SQL Server**
- The `DBSetup.sql` script executed to create the `StoreSample` database

## 🛠️ Database Setup

1. Install or access SQL Server.
2. Execute `DBSetup.sql` to initialize the `StoreSample` database.
3. Ensure the connection string in `appsettings.json` is valid for your environment:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=StoreSample;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

## 🚀 Build & Run

Follow these steps to run the project locally:

```bash
# Clone the repository
git clone https://github.com/thoughtdirector/sales-date-prediction-brackend
cd SalesDatePrediction

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run unit tests
dotnet test

# Run the Web API
cd SalesDatePrediction.API
dotnet run
```

Once running, the API will be accessible via:
- **HTTPS**: https://localhost:7299/index.html 

## 📡 API Endpoints Overview

### Customers & Predictions

- `GET /api/customers/predictions?nameFilter={name}`  
  Returns a list of customers with their last order date and predicted next order date. Supports name-based filtering.

### Orders

- `GET /api/orders/customer/{customerId}`  
  Retrieves all orders placed by a specific customer.

- `POST /api/orders`  
  Creates a new order including at least one product.

### Employees

- `GET /api/employees`  
  Returns all employee records.

### Shippers

- `GET /api/shippers`  
  Returns the list of shipping providers.

### Products

- `GET /api/products`  
  Returns the list of all available products.

## 💡 SQL Query Logic

The following SQL operations are implemented within the application:

1. **Sales Date Prediction** – Predicts each customer's next order date by calculating the average interval between past orders and adding it to the last order date.
2. **Get Client Orders** – Fetches all orders for a given customer.
3. **Get Employees** – Returns employee names using concatenation of first and last names.
4. **Get Shippers** – Retrieves all shipping companies.
5. **Get Products** – Returns product data.
6. **Add New Order** – Creates a new order and adds a product to it via a transactional SQL script.

## 🔧 Technologies Used

- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **Dapper** 
- **SQL Server**
- **Swagger** 
- **Repository Pattern**
- **Clean Architecture & SOLID Principles**

## 📘 Additional Notes

- All core logic is cleanly separated and testable.
- Predictive logic is implemented at the database level for performance and accuracy.
- The API supports pagination and server-side filtering where appropriate.
