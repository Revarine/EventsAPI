# EventsAPI

A .NET-based Events Management API that allows users to create, manage, and participate in events.

## Prerequisites

- .NET 8.0 SDK
- SQLite
- Docker (optional, for containerized deployment)

## Project Structure

```
EventsAPI/
├── src/
│   ├── Events.API/         # API endpoints and configuration
│   ├── Events.Application/ # Application logic, CQRS handlers
│   ├── Events.Domain/      # Domain entities and business logic
│   └── Events.Infrastructure/ # Data access and external services
├── tests/
│   └── Events.UnitTests/   # Unit tests
├── Dockerfile
└── docker-compose.yml
```

## Running Locally

1. Clone the repository:
```bash
git clone <repository-url>
cd EventsAPI
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

4. Run the tests:
```bash
dotnet test
```

5. Run the application:
```bash
cd src/Events.API
dotnet run
```

The API will be available at `http://localhost:5000`.

## Running with Docker

1. Clone the repository:
```bash
git clone <repository-url>
cd EventsAPI
```

2. Build and run using Docker Compose:
```bash
docker-compose build
docker-compose up
```

The API will be available at `http://localhost:5000`.

### Docker Environment

- The application runs in a containerized environment
- SQLite database is persisted using Docker volumes
- Environment variables and connection strings are configured in docker-compose.yml

To stop the containers:
```bash
docker-compose down
```

To remove all containers and volumes:
```bash
docker-compose down -v
```

## Testing

The project includes unit tests for:
- Domain entities
- Repositories
- Application services

Run the tests using:
```bash
dotnet test
```

## Database

The application uses SQLite as its database. The connection string can be configured in:
- `appsettings.json` for local development
- `docker-compose.yml` for containerized deployment

## API Documentation

Once the application is running, you can access the Swagger documentation at:
- Local: `http://localhost:5000/swagger`
- Docker: `http://localhost:5000/swagger`
