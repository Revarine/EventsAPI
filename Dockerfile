FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY *.sln .
COPY src/Events.Application/*.csproj src/Events.Application/
COPY src/Events.Domain/*.csproj src/Events.Domain/
COPY src/Events.Infrastructure/*.csproj src/Events.Infrastructure/
COPY tests/Events.UnitTests/*.csproj tests/Events.UnitTests/
RUN dotnet restore

# Copy the rest of the files and build
COPY . .
RUN dotnet build -c Release --no-restore
RUN dotnet test -c Release --no-build
RUN dotnet publish -c Release -o /app/publish --no-build src/Events.API/Events.API.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Create volume for SQLite database
VOLUME /app/data

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Events.API.dll"] 