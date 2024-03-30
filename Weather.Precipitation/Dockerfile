FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /source
COPY . .
RUN dotnet restore "./Weather.Precipitation.csproj" --disable-parallel
RUN dotnet publish "./Weather.Precipitation.csproj" -c Release -o /app --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build-env /app ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "Weather.Precipitation.dll"]
