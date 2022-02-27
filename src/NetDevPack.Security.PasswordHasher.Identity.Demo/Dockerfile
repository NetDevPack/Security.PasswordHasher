#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NetDevPack.Security.PasswordHasher.Identity.Demo/NetDevPack.Security.PasswordHasher.Identity.Demo.csproj", "NetDevPack.Security.PasswordHasher.Identity.Demo/"]
RUN dotnet restore "NetDevPack.Security.PasswordHasher.Identity.Demo/NetDevPack.Security.PasswordHasher.Identity.Demo.csproj"
COPY . .
WORKDIR "/src/NetDevPack.Security.PasswordHasher.Identity.Demo"
RUN dotnet build "NetDevPack.Security.PasswordHasher.Identity.Demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetDevPack.Security.PasswordHasher.Identity.Demo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetDevPack.Security.PasswordHasher.Identity.Demo.dll"]