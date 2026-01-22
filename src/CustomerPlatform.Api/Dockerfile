FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["CustomerPlatform/src/CustomerPlatform.Api/CustomerPlatform.Api.csproj", "CustomerPlatform/src/CustomerPlatform.Api/"]
COPY ["CustomerPlatform/src/CustomerPlatform.Application/CustomerPlatform.Application.csproj", "CustomerPlatform/src/CustomerPlatform.Application/"]
COPY ["CustomerPlatform/src/CustomerPlatform.Domain/CustomerPlatform.Domain.csproj", "CustomerPlatform/src/CustomerPlatform.Domain/"]
COPY ["CustomerPlatform/src/CustomerPlatform.Infrastructure/CustomerPlatform.Infrastructure.csproj", "CustomerPlatform/src/CustomerPlatform.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "CustomerPlatform/src/CustomerPlatform.Api/CustomerPlatform.Api.csproj"

# Copiar todo o código fonte
COPY CustomerPlatform/src/ CustomerPlatform/src/

# Build
WORKDIR "/src/CustomerPlatform/src/CustomerPlatform.Api"
RUN dotnet build "CustomerPlatform.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerPlatform.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerPlatform.Api.dll"]
