FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TournamentPlatform.API/TournamentPlatform.API.csproj", "TournamentPlatform.API/"]
COPY ["TournamentPlatform.Application/TournamentPlatform.Application.csproj", "TournamentPlatform.Application/"]
COPY ["TournamentPlatform.Domain/TournamentPlatform.Domain.csproj", "TournamentPlatform.Domain/"]
COPY ["TournamentPlatform.Infrastructure/TournamentPlatform.Infrastructure.csproj", "TournamentPlatform.Infrastructure/"]
RUN dotnet restore "TournamentPlatform.API/TournamentPlatform.API.csproj"
COPY . .
WORKDIR "/src/TournamentPlatform.API"
RUN dotnet build "TournamentPlatform.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TournamentPlatform.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TournamentPlatform.API.dll"]
