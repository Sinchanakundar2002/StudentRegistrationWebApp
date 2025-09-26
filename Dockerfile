# Use Windows-based .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022
WORKDIR C:\app

# Copy csproj and restore first (better caching)
COPY *.csproj .\
RUN dotnet restore

# Copy the rest of the project
COPY . .\

# Publish the app
RUN dotnet publish -c Release -o C:\app\publish

# Switch to publish folder
WORKDIR C:\app\publish

# Expose port
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "MyFirstWebApp1.dll"]
