# Fase 1: De 'Build' omgeving
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Kopieer project- en solution-bestanden voor caching
COPY ["ECommerceSolution.sln", "."]
COPY ["src/ECommerce.Orders.Api/ECommerce.Orders.Api.csproj", "src/ECommerce.Orders.Api/"]
COPY ["src/ECommerce.Orders.Application/ECommerce.Orders.Application.csproj", "src/ECommerce.Orders.Application/"]
COPY ["src/ECommerce.Orders.Domain/ECommerce.Orders.Domain.csproj", "src/ECommerce.Orders.Domain/"]
COPY ["src/ECommerce.Orders.Infrastructure/ECommerce.Orders.Infrastructure.csproj", "src/ECommerce.Orders.Infrastructure/"]
COPY ["src/ECommerce.Orders.Worker/ECommerce.Orders.Worker.csproj", "src/ECommerce.Orders.Worker/"]

# Herstel NuGet packages
RUN dotnet restore "ECommerceSolution.sln"

# Kopieer de rest van de code
COPY . .

# Publiceer de API
RUN dotnet publish "src/ECommerce.Orders.Api/ECommerce.Orders.Api.csproj" -c Release -o /app/publish


# Fase 2: De 'Final' productieomgeving
# We gebruiken de 'aspnet' runtime, want dit is een web applicatie
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Kopieer de gepubliceerde output
COPY --from=build /app/publish .

# Start de API
ENTRYPOINT ["dotnet", "ECommerce.Orders.Api.dll"]