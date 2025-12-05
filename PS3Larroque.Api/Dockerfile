# Imagen base para correr la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Imagen para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos todo el código al contenedor
COPY . .

# Restauramos paquetes
RUN dotnet restore "PS3Larroque.Api.csproj"

# Publicamos en modo Release
RUN dotnet publish "PS3Larroque.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app

# Copiamos lo publicado
COPY --from=build /app/publish .

# Exponemos un puerto (Render después setea PORT y nosotros lo manejamos en Program.cs)
EXPOSE 8080

# Ejecutamos la API
ENTRYPOINT ["dotnet", "PS3Larroque.Api.dll"]
