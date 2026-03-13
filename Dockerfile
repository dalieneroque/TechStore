FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/TechStore.API/TechStore.API.csproj", "src/TechStore.API/"]
COPY ["src/TechStore.Application/TechStore.Application.csproj", "src/TechStore.Application/"]
COPY ["src/TechStore.Core/TechStore.Core.csproj", "src/TechStore.Core/"]
COPY ["src/TechStore.Infrastructure/TechStore.Infrastructure.csproj", "src/TechStore.Infrastructure/"]

RUN dotnet restore "src/TechStore.API/TechStore.API.csproj"

COPY . .

WORKDIR "/src/src/TechStore.API"

RUN dotnet build "TechStore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechStore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TechStore.API.dll"]
