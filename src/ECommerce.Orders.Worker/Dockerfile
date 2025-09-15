# Fase 1: De 'Build' omgeving  
## zet de werkdirectory in de container
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

## Kopieer eerst de projectbestanden (.csproj) en het solution-bestand (.sln)
COPY ["ECommerceSolution.sln", "."]
COPY ["src/ECommerce.Orders.Api/ECommerce.Orders.Api.csproj", "src/ECommerce.Orders.Api/"]
COPY ["src/ECommerce.Orders.Application/ECommerce.Orders.Application.csproj", "src/ECommerce.Orders.Application/"]
COPY ["src/ECommerce.Orders.Domain/ECommerce.Orders.Domain.csproj", "src/ECommerce.Orders.Domain/"]
COPY ["src/ECommerce.Orders.Infrastructure/ECommerce.Orders.Infrastructure.csproj", "src/ECommerce.Orders.Infrastructure/"]
COPY ["src/ECommerce.Orders.Worker/ECommerce.Orders.Worker.csproj", "src/ECommerce.Orders.Worker/"]

## Herstel alle NuGet packages
RUN dotnet restore "ECommerceSolution.sln"

## Kopieer de rest van de broncode
COPY . .

# Publiceer de Worker
RUN dotnet publish "src/ECommerce.Orders.Worker/ECommerce.Orders.Worker.csproj" -c Release -o /app/publish

# Fase 2: De 'Final' productieomgeving
## Zet de werkdirectory in de container
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

## Kopieer alleen de gepubliceerde output van de 'build' fase
COPY --from=build /app/publish .

## Start de Worker
ENTRYPOINT ["dotnet", "ECommerce.Orders.Worker.dll"]