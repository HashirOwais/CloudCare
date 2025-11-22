ARG DOTNET_VERSION=9.0 

# ─── Step 1: Build the API ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copy the entire solution directory
COPY CloudCare-API/ . 

# Restore dependencies
RUN dotnet restore "CloudCare.API.sln"

# Publish the application
WORKDIR "/src/CloudCare.API"
RUN dotnet publish -c Release -o /app/publish

# ─── Step 2: Create the SDK-enabled runtime image ──────────────────
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Environment and port
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Run the API
ENTRYPOINT ["dotnet", "CloudCare.API.dll"]

