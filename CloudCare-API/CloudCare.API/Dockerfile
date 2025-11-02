ARG DOTNET_VERSION=9.0 

# ─── Step 1: Build the API ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /app

# Copy solution and project files
COPY CloudCare-API/CloudCare.API.sln .
COPY CloudCare-API/CloudCare.API/CloudCare.API.csproj ./CloudCare-API/CloudCare.API/
COPY CloudCare-API/CloudCare.API.Tests/CloudCare.API.Tests.csproj ./CloudCare-API/CloudCare.API.Tests/

# Restore dependencies
RUN dotnet restore "CloudCare.API.sln"

# Copy the rest of your source code and publish
COPY . .
WORKDIR "/app/CloudCare-API/CloudCare.API"
RUN dotnet publish -c Release -o /app/publish

# ─── Step 2: Create the runtime image ──────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Environment and port
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Run the API
ENTRYPOINT ["dotnet", "CloudCare.API.dll"]