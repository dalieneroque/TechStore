# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solução e os csproj primeiro (melhora cache)
COPY TechStore.sln ./
COPY src/TechStore.API/TechStore.API.csproj src/TechStore.API/
COPY src/TechStore.Application/TechStore.Application.csproj src/TechStore.Application/
COPY src/TechStore.Infrastructure/TechStore.Infrastructure.csproj src/TechStore.Infrastructure/
COPY src/TechStore.Core/TechStore.Core.csproj src/TechStore.Core/

RUN dotnet restore src/TechStore.API/TechStore.API.csproj

# Copia o restante do código
COPY . .

RUN dotnet publish src/TechStore.API/TechStore.API.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "TechStore.API.dll"]
