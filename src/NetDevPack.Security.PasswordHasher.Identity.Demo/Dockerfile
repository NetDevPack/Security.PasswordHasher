# Build context is the repository root (Directory.Build.props, Directory.Packages.props, global.json):
#   docker build -f src/NetDevPack.Security.PasswordHasher.Identity.Demo/Dockerfile .

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /repo
COPY ["Directory.Build.props", "Directory.Packages.props", "global.json", "icon.png", "README.md", "./"]
COPY ["src/", "src/"]
RUN dotnet restore "src/NetDevPack.Security.PasswordHasher.Identity.Demo/NetDevPack.Security.PasswordHasher.Identity.Demo.csproj"
WORKDIR "/repo/src/NetDevPack.Security.PasswordHasher.Identity.Demo"
RUN dotnet build "NetDevPack.Security.PasswordHasher.Identity.Demo.csproj" -c Release -o /app/build -f net10.0

FROM build AS publish
RUN dotnet publish "NetDevPack.Security.PasswordHasher.Identity.Demo.csproj" -c Release -o /app/publish -f net10.0

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetDevPack.Security.PasswordHasher.Identity.Demo.dll"]
