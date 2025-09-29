# =============================
# 1. Build stage
# =============================
FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022 AS build

# Set working directory (double backslashes for Windows paths)
WORKDIR C:\\src

# Copy only project files first (caching optimization)
COPY *.csproj .\\

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .\\

# Publish the app to a short path with app host disabled
RUN dotnet publish -c Release -o C:\\app\\publish /p:UseAppHost=false

# =============================
# 2. Runtime stage
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0-windowsservercore-ltsc2022 AS runtime

# Set working directory
WORKDIR C:\\app

# Copy published files from build stage
COPY --from=build C:\\app\\publish .\\

# Expose application port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "MyFirstWebApp1.dll"]
