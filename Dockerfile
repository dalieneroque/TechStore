FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de solução e projetos
COPY ["src/TechStore.API/TechStore.API.csproj", "src/TechStore.API/"]
COPY ["src/TechStore.Application/TechStore.Application.csproj", "src/TechStore.Application/"]
COPY ["src/TechStore.Core/TechStore.Core.csproj", "src/TechStore.Core/"]
COPY ["src/TechStore.Infrastructure/TechStore.Infrastructure.csproj", "src/TechStore.Infrastructure/"]

# Restore
RUN dotnet restore "src/TechStore.API/TechStore.API.csproj"

# Copiar todo o código
COPY src/ ./src/

# Publicar
FROM build AS publish
WORKDIR /src/src/TechStore.API
RUN dotnet publish "TechStore.API.csproj" -c Release -o /app/publish --no-restore

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar os arquivos publicados
COPY --from=publish /app/publish .

# Se o appsettings.json não existir, crie um arquivo vazio
RUN if [ ! -f appsettings.json ]; then echo "{}" > appsettings.json; fi

# Se o appsettings.Production.json não existir, não faz nada
RUN if [ ! -f appsettings.Production.json ]; then echo "Configuração de produção não encontrada, usando appsettings.json"; fi

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TechStore.API.dll"]