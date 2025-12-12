# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# COPY CSPROJ FILES (Keep this part, it's good)
COPY ["src/CloudCare.API/CloudCare.API.csproj", "src/CloudCare.API/"]
COPY ["src/CloudCare.Business/CloudCare.Business.csproj", "src/CloudCare.Business/"]
COPY ["src/CloudCare.Data/CloudCare.Data.csproj", "src/CloudCare.Data/"]

# Restore
RUN dotnet restore "src/CloudCare.API/CloudCare.API.csproj"

# COPY EVERYTHING
COPY . .

# --- CRITICAL FIX ---
# Don't change WORKDIR. Stay in /app and use the full path.
# This ensures Linux runners find the file exactly where you put it.
RUN dotnet publish "src/CloudCare.API/CloudCare.API.csproj" -c Release -o /app/out

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "CloudCare.API.dll"]