# Use the official .NET SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore as distinct layers
COPY . ./
RUN dotnet restore MarketPlaceCore/src/MarketPlaceCore.Web/MarketPlaceCore.Web.csproj

# Build and publish a release
RUN dotnet publish MarketPlaceCore/src/MarketPlaceCore.Web/MarketPlaceCore.Web.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port 80
EXPOSE 80

# Set entry point
ENTRYPOINT ["dotnet", "MarketPlaceCore.Web.dll"]
