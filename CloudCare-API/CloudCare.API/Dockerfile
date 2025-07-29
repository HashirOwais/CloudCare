# ─── Step 1: Build the API ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the project file first and restore
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of your source code and publish
COPY . . 
RUN dotnet publish -c Release -o /app/publish

# ─── Step 2: Create the runtime image ──────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .



# Environment and port
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Run the API
ENTRYPOINT ["dotnet", "CloudCare.API.dll"]
