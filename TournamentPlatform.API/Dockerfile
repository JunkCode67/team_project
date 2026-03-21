FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TournamentPlatform.API/TournamentPlatform.API.csproj", "TournamentPlatform.API/"]
COPY ["TournamentPlatform.Application/TournamentPlatform.Application.csproj", "TournamentPlatform.Application/"]
COPY ["TournamentPlatform.Domain/TournamentPlatform.Domain.csproj", "TournamentPlatform.Domain/"]
COPY ["TournamentPlatform.Infrastructure/TournamentPlatform.Infrastructure.csproj", "TournamentPlatform.Infrastructure/"]
RUN dotnet restore "TournamentPlatform.API/TournamentPlatform.API.csproj"
COPY . .
WORKDIR "/src/TournamentPlatform.API"
RUN dotnet build "./TournamentPlatform.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TournamentPlatform.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TournamentPlatform.API.dll"]
