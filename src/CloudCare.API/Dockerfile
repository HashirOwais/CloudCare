# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# COPY CSPROJ FILES FIRST (For caching optimization)
COPY ["src/CloudCare.API/CloudCare.API.csproj", "src/CloudCare.API/"]
COPY ["src/CloudCare.Business/CloudCare.Business.csproj", "src/CloudCare.Business/"]
COPY ["src/CloudCare.Data/CloudCare.Data.csproj", "src/CloudCare.Data/"]

# Restore dependencies
RUN dotnet restore "src/CloudCare.API/CloudCare.API.csproj"

# COPY EVERYTHING ELSE
COPY . .

# Build and Publish
WORKDIR "/app/src"
RUN dotnet publish "CloudCare.API/CloudCare.API.csproj" -c Release -o /app/out

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "CloudCare.API.dll"]