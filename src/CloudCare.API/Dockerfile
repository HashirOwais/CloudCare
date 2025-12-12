# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# COPY CSPROJ FILES
COPY ["src/CloudCare.API/CloudCare.API.csproj", "src/CloudCare.API/"]
COPY ["src/CloudCare.Business/CloudCare.Business.csproj", "src/CloudCare.Business/"]
COPY ["src/CloudCare.Data/CloudCare.Data.csproj", "src/CloudCare.Data/"]

# Restore
RUN dotnet restore "src/CloudCare.API/CloudCare.API.csproj"

# COPY EVERYTHING
COPY . .

# Build and Publish
RUN dotnet publish "src/CloudCare.API/CloudCare.API.csproj" -c Release -o /app/out

# 2. Runtime Stage (THIS WAS MISSING THE NAME "runtime")
# We add "AS runtime" so the YAML knows which stage to target.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "CloudCare.API.dll"]
