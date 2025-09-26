# =============================
# 1. Build stage
# =============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code
COPY . ./

# Publish the app to /app folder
RUN dotnet publish -c Release -o /app/publish

# =============================
# 2. Runtime stage
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Expose port 8080
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "MyFirstWebApp1.dll"]
