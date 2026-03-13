FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de solução e projetos
COPY ["src/TechStore.API/TechStore.API.csproj", "src/TechStore.API/"]
COPY ["src/TechStore.Application/TechStore.Application.csproj", "src/TechStore.Application/"]
COPY ["src/TechStore.Core/TechStore.Core.csproj", "src/TechStore.Core/"]
COPY ["src/TechStore.Infrastructure/TechStore.Infrastructure.csproj", "src/TechStore.Infrastructure/"]

# Restore
RUN dotnet restore "src/TechStore.API/TechStore.API.csproj"

# Copiar todo o código e arquivos de configuração
COPY src/ ./src/

# IMPORTANTE: Copiar appsettings.json explicitamente
COPY src/TechStore.API/appsettings.json ./src/TechStore.API/appsettings.json
COPY src/TechStore.API/appsettings.Production.json ./src/TechStore.API/appsettings.Production.json 2>/dev/null || true

# Publicar
FROM build AS publish
WORKDIR /src/src/TechStore.API
RUN dotnet publish "TechStore.API.csproj" -c Release -o /app/publish --no-restore

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar os arquivos publicados
COPY --from=publish /app/publish .

# Garantir que os arquivos de configuração estão presentes
COPY --from=build /src/src/TechStore.API/appsettings.json ./appsettings.json 2>/dev/null || true

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TechStore.API.dll"]