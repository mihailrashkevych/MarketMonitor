#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MarketMonitor/MarketMonitor.API.csproj", "MarketMonitor/"]
COPY ["MarketMonitor.Core.Contracts/MarketMonitor.Core.Contracts.csproj", "MarketMonitor.Core.Contracts/"]
COPY ["MarketMonitor.RI.Contracts/MarketMonitor.RI.Contracts.csproj", "MarketMonitor.RI.Contracts/"]
COPY ["MarketMonitor.Core/MarketMonitor.Core.csproj", "MarketMonitor.Core/"]
COPY ["MarketMonitor.DataProvider/MarketMonitor.DataProvider.csproj", "MarketMonitor.DataProvider/"]
COPY ["MarketMonitor.DataStore.SQL/MarketMonitor.DataStore.SQL.csproj", "MarketMonitor.DataStore.SQL/"]
RUN dotnet restore "./MarketMonitor/MarketMonitor.API.csproj"
COPY . .
WORKDIR "/src/MarketMonitor"
RUN dotnet build "./MarketMonitor.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MarketMonitor.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketMonitor.API.dll"]