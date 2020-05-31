FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY olmelabs.battleship.api/olmelabs.battleship.api.csproj olmelabs.battleship.api/
RUN dotnet restore olmelabs.battleship.api/olmelabs.battleship.api.csproj
COPY . .
WORKDIR /src/olmelabs.battleship.api
RUN dotnet build olmelabs.battleship.api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish olmelabs.battleship.api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "olmelabs.battleship.api.dll"]
