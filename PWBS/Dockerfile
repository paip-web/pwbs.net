FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PWBS/PWBS.csproj", "PWBS/"]
RUN dotnet restore "PWBS/PWBS.csproj"
COPY . .
WORKDIR "/src/PWBS"
RUN dotnet build "PWBS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PWBS.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PWBS.dll"]
