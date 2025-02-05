FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Create and configure NuGet
RUN mkdir -p /root/.nuget/NuGet
RUN echo '<?xml version="1.0" encoding="utf-8"?>\n\
<configuration>\n\
  <packageSources>\n\
    <clear />\n\
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />\n\
  </packageSources>\n\
  <fallbackPackageFolders>\n\
    <clear />\n\
  </fallbackPackageFolders>\n\
</configuration>' > /root/.nuget/NuGet/NuGet.Config

# Copy csproj files and restore dependencies
COPY *.sln .
COPY src/Events.API/*.csproj src/Events.API/
COPY src/Events.Application/*.csproj src/Events.Application/
COPY src/Events.Domain/*.csproj src/Events.Domain/
COPY src/Events.Infrastructure/*.csproj src/Events.Infrastructure/
COPY tests/Events.UnitTests/*.csproj tests/Events.UnitTests/

# Restore packages
RUN dotnet restore

# Copy the rest of the files and build
COPY . .
RUN dotnet build -c Release
RUN dotnet test -c Release
RUN dotnet publish -c Release -o /app/publish src/Events.API/Events.API.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Create necessary directories
RUN mkdir -p /root/.aspnet/DataProtection-Keys
RUN mkdir -p /app/data

# Copy published files
COPY --from=build /app/publish .

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create volumes for persistence
VOLUME /root/.aspnet/DataProtection-Keys
VOLUME /app/data

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Events.API.dll"] 