# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["ECommerce.Api/ECommerce.Api.csproj", "ECommerce.Api/"]
# (herhaal eventueel voor andere project references, bv. Orders, Core, etc.)
RUN dotnet restore "ECommerce.Api/ECommerce.Api.csproj"

# Copy rest of the source
COPY . .

# Build and publish
WORKDIR "/src/ECommerce.Api"
RUN dotnet publish "ECommerce.Api.csproj" -c Release -o /app/publish

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.Api.dll"]