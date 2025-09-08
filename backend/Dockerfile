FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["backend/RH-App.API.csproj", "backend/"]
RUN dotnet restore "backend/RH-App.API.csproj"
COPY . .
WORKDIR "/src/backend"
RUN dotnet build "RH-App.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RH-App.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RH-App.API.dll"]