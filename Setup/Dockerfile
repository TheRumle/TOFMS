FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Setup/Setup.csproj", "Setup/"]
RUN dotnet restore "Setup/Setup.csproj"
COPY . .
WORKDIR "/src/Setup"
RUN dotnet build "Setup.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Setup.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Setup.dll"]
