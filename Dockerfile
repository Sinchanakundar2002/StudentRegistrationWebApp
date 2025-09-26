# =============================
# 1. Build stage
# =============================
FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022 AS build
# Use double backslashes for absolute paths
WORKDIR C:\\src

# Copy csproj and restore first (Docker cache optimization)
COPY *.csproj .\\
RUN dotnet restore

# Copy the rest of the project
COPY . .\\

# Publish the app to C:\app\publish
RUN dotnet publish -c Release -o C:\\app\\publish

# =============================
# 2. Runtime stage
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0-windowsservercore-ltsc2022 AS runtime
WORKDIR C:\\app

# Copy published files from build stage
COPY --from=build C:\\app\\publish .\\

# Expose port
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "MyFirstWebApp1.dll"]
