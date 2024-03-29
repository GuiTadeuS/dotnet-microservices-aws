FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /source
COPY . .
RUN dotnet restore "./Weather.DataLoader.csproj" --disable-parallel
RUN dotnet publish "./Weather.DataLoader.csproj" -c Release -o /app --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build-env /app ./

ENTRYPOINT ["dotnet", "Weather.DataLoader.dll"]
