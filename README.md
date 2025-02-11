# Prophecy International: Call Center Management API

**Author:** Jehiel Camille Balla-Sabillon

## Overview
This project is a **Call Center Management API** built using **ASP.NET Core (.NET 6 or later)**. It demonstrates RESTful API design, database interactions, authentication, and testing.

## Features
- CRUD operations for **Agents, Calls, Customers, and Tickets**.
- **Entity Framework Core** for database operations.
- **JWT authentication** for secure access.
- **Unit tests** for service layer.
- **Integration tests** for API endpoints.
- **Logging and error handling**.
- **Dependency Injection** for maintainability.
- **Bruno** as the API client tool.

## Tech Stack
- **.NET 6 or later** (ASP.NET Core Web API)
- **Entity Framework Core** (Database)
- **JWT Authentication** (Security)
- **xUnit** (Unit Testing)
- **Swagger (Swashbuckle)** (API Documentation)
- **Bruno** (REST API Client)

## Data Models

### Agent
| Property       | Type   | Description        |
|---------------|--------|--------------------|
| Id            | int    | Unique identifier |
| Name          | string | Agent's name      |
| Email         | string | Agent's email     |
| PhoneExtension | string | Phone extension   |
| Status        | enum   | Available, Busy, Offline |

### Call
| Property    | Type    | Description        |
|------------|---------|--------------------|
| Id         | int     | Unique identifier |
| CustomerId | string  | Related customer  |
| AgentId    | int?    | Assigned agent    |
| StartTime  | DateTime | Call start time  |
| EndTime    | DateTime? | Call end time   |
| Status     | enum    | Queued, InProgress, Completed, Dropped |
| Notes      | string  | Additional info   |

### Customer
| Property        | Type    | Description       |
|----------------|---------|-------------------|
| Id             | string  | Unique identifier |
| Name           | string  | Customer's name  |
| Email          | string  | Customer email   |
| PhoneNumber    | string  | Contact number   |
| LastContactDate | DateTime? | Last contact |

### Ticket
| Property    | Type     | Description       |
|------------|----------|-------------------|
| Id         | int      | Unique identifier |
| CustomerId | string   | Related customer  |
| AgentId    | int?     | Assigned agent    |
| Status     | enum     | Open, InProgress, Resolved, Closed |
| Priority   | enum     | Low, Medium, High, Urgent |
| CreatedAt  | DateTime | Creation timestamp |
| UpdatedAt  | DateTime | Last update timestamp |
| Description | string  | Issue details     |
| Resolution  | string? | Resolution notes  |

## API Endpoints

### Agents
- `GET /agents` - Retrieve all agents
- `GET /agents/{id}` - Retrieve a specific agent
- `POST /agents` - Add a new agent
- `PUT /agents/{id}` - Update an existing agent
- `DELETE /agents/{id}` - Delete an agent
- `PATCH /agents/{id}/status` - Update agent status

### Calls
- `GET /calls` - Retrieve all calls
- `GET /calls/{id}` - Retrieve a specific call
- `POST /calls` - Create a new call
- `PUT /calls/{id}` - Update an existing call
- `DELETE /calls/{id}` - Delete a call
- `PATCH /calls/{id}/assign` - Assign a call to an agent

### Customers
- `GET /customers` - Retrieve all customers
- `GET /customers/{id}` - Retrieve a specific customer
- `POST /customers` - Add a new customer
- `PUT /customers/{id}` - Update an existing customer
- `DELETE /customers/{id}` - Delete a customer

### Tickets
- `GET /tickets` - Retrieve all tickets
- `GET /tickets/{id}` - Retrieve a specific ticket
- `POST /tickets` - Create a new ticket
- `PUT /tickets/{id}` - Update an existing ticket
- `DELETE /tickets/{id}` - Delete a ticket
- `PATCH /tickets/{id}/assign` - Assign a ticket to an agent

## Authentication
- **JWT Authentication** is implemented.
- **Secured Endpoints**: All except retrieving calls.

## Testing
- **Unit Tests** for service layer.
- **Integration Tests** for API endpoints.

## Setup & Installation
### Prerequisites
- Visual Studio
- .NET 6 or later
- SQL Server / PostgreSQL
- Bruno API Client

## Folder Structure
- **Common Components**: ProphecyInternational.Common
- **Database**: PProphecyInternational.Database
- **Server**: ProphecyInternational.Server
- **Bruno Files**: ProphecyInternational.BrunoFiles

## Configurations
The following settings could be configured in _ProphecyInternational.Server->appsettings.json_ file

- **ConnectionStrings:DefaultConnection** - Database connection string
- **Jwt:Username** - Username for login
- **Jwt:Password** - Password for login

**NOTE**: Add-on features can be found on _dev/addons_ branch

