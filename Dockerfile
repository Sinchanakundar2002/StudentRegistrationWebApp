# =============================
# 1. Build stage
# =============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory
WORKDIR /src

# Copy only project file first (for layer caching)
COPY *.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Publish the app (no app host for smaller image)
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# =============================
# 2. Runtime stage
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set working directory
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose application port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "MyFirstWebApp1.dll"]
