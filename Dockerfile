# Imagen base para correr la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Imagen para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos toda la solución
COPY . .

# Restauramos paquetes de la API (esto arrastra los demás proyectos)
RUN dotnet restore "PS3Larroque.Api/PS3Larroque.Api.csproj"

# Publicamos en modo Release
RUN dotnet publish "PS3Larroque.Api/PS3Larroque.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PS3Larroque.Api.dll"]
