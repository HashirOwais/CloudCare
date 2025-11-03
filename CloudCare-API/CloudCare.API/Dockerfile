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

# ─── Step 3: Create a migration utility image ──────────────────────
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS migration
WORKDIR /src

# Install the dotnet-ef Global Tool
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the entire solution directory (Assuming CloudCare-API is a folder here)
COPY CloudCare-API/ .

# FIX: Use paths relative to the current WORKDIR (/src)
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "CloudCare.API/CloudCare.API.csproj", "--startup-project", "CloudCare.API"]
# NOTE: For the --startup-project, the folder name (CloudCare.API) is often sufficient, 
# or you can simply remove it if the project structure is flat and the DbContext is 
# in the API project, as the --project argument is already specified.