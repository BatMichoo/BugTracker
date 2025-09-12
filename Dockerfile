FROM mcr.microsoft.com/dotnet/sdk:8.0.100-alpine AS build
WORKDIR /src
COPY ["BugTracker.sln", "."]
COPY ["API/API.csproj", "API/"]
RUN dotnet restore "API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:IncludeNativeLibrariesForSelfExtract=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0.100-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
