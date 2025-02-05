# EventsAPI

A .NET-based Events Management API that allows users to create, manage, and participate in events.

## Prerequisites

- .NET 8.0 SDK
- SQLite
- Docker (optional, for containerized deployment)
- Docker Compose (optional, for containerized deployment)

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
├── Dockerfile             # Docker build instructions
└── docker-compose.yml    # Docker Compose configuration
```
## Preconfiguring
adjust `appsettings.json` for full experience, like smtp, db name and etc.
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

### Prerequisites
- Docker Desktop installed and running
- Docker Compose installed (comes with Docker Desktop)

### Steps

1. Clone the repository:
```bash
git clone <repository-url>
cd EventsAPI
```

2. Build and run using Docker Compose:
```bash
# Build the containers
docker-compose build

# Run the containers in the background
docker-compose up -d
```

The API will be available at `http://localhost:5000`.

### Docker Environment Details

- The application runs in a containerized environment
- SQLite database is persisted using Docker volumes at `/app/data`
- Environment variables and connection strings are configured in docker-compose.yml
- Health checks are configured to ensure service availability
- Container automatically restarts unless explicitly stopped

### Docker Commands

Start the application:
```bash
docker-compose up -d
```

View logs:
```bash
docker-compose logs -f
```

Stop the application:
```bash
docker-compose down
```

Remove containers and volumes:
```bash
docker-compose down -v
```

### Troubleshooting Docker

1. If the container fails to start:
   ```bash
   # Check container logs
   docker-compose logs api
   ```

2. If you need to rebuild:
   ```bash
   # Force a rebuild
   docker-compose build --no-cache
   ```

3. If you need to reset the database:
   ```bash
   # Remove volumes and rebuild
   docker-compose down -v
   docker-compose up -d
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

Database file locations:
- Local: `Events.db` in the application root
- Docker: `/app/data/Events.db` inside the container, persisted through Docker volume

## API Documentation

Once the application is running, you can access the Swagger documentation at:
- Local: `http://localhost:5000/swagger`
- Docker: `http://localhost:5000/swagger`
