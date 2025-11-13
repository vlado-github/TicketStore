# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["TicketStore.API/TicketStore.API.csproj", "TicketStore.API/"]
COPY ["TicketStore.Domain/TicketStore.Domain.csproj", "TicketStore.Domain/"]

# Restore dependencies
RUN dotnet restore "TicketStore.API/TicketStore.API.csproj"

# Copy the rest of the source
COPY . . 

# Build the application
WORKDIR "/src/TicketStore.API"
RUN dotnet build "TicketStore.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TicketStore.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entrypoint
ENTRYPOINT ["dotnet", "TicketStore.API.dll"]