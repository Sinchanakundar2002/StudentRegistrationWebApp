FROM mcr.microsoft.com/dotnet/sdk:9.0-windowsservercore-ltsc2022 AS build
WORKDIR C:\\src

COPY *.csproj .\
RUN dotnet restore

COPY . .\
RUN dotnet publish -c Release -o C:\\app\\publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-windowsservercore-ltsc2022 AS runtime
WORKDIR C:\\app
COPY --from=build C:\\app\\publish .\

EXPOSE 8080
ENTRYPOINT ["dotnet", "MyFirstWebApp1.dll"]
