# ---------------------------------------
# 1. ETAPA DE CONSTRUCCIÓN (Build)
# ---------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos primero los archivos de proyecto (.csproj) y la solución (.sln)
# Esto optimiza la caché de Docker: si no cambias dependencias, este paso es instantáneo.
COPY ["UserAuthenticationService.sln", "."]
COPY ["UserAuthenticationService.API/UserAuthenticationService.API.csproj", "UserAuthenticationService.API/"]
COPY ["UserAuthenticationService.Application/UserAuthenticationService.Application.csproj", "UserAuthenticationService.Application/"]
COPY ["UserAuthenticationService.Domain/UserAuthenticationService.Domain.csproj", "UserAuthenticationService.Domain/"]
COPY ["UserAuthenticationService.Infrastructure/UserAuthenticationService.Infrastructure.csproj", "UserAuthenticationService.Infrastructure/"]

# 2. Restauramos las dependencias
RUN dotnet restore "UserAuthenticationService.sln"

# 3. Copiamos el resto del código fuente
COPY . .

# 4. Nos movemos a la carpeta de la API para publicar
WORKDIR "/src/UserAuthenticationService.API"
RUN dotnet publish "UserAuthenticationService.API.csproj" -c Release -o /app/publish

# ---------------------------------------
# 2. ETAPA DE EJECUCIÓN (Runtime)
# ---------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiamos los archivos compilados de la etapa anterior
COPY --from=build /app/publish .

# Comando de inicio
ENTRYPOINT ["dotnet", "UserAuthenticationService.API.dll"]