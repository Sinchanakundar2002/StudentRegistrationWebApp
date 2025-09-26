# Use Windows-based .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022
WORKDIR C:\app

# Copy the entire project
COPY . ./

# Restore dependencies and publish the app
RUN dotnet restore
RUN dotnet publish -c Release -o C:\app\publish

# Set working directory to published output
WORKDIR C:\app\publish

# Expose port 8080
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "MyFirstWebApp1.dll"]
