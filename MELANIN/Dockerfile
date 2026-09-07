# ===== Étape 1 : BUILD (grosse boîte SDK) =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish Melanin.API/Melanin.API.csproj -c Release -o out

# ===== Étape 2 : RUNTIME (petite boîte aspnet) =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /src/out .
ENTRYPOINT ["dotnet", "Melanin.API.dll"]