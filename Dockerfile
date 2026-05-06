FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["GymApi/GymApi/GymApi.csproj", "GymApi/GymApi/"]
COPY ["GymApi/GymApi.Application/GymApi.Application.csproj", "GymApi/GymApi.Application/"]
COPY ["GymApi/GymApi.Domain/GymApi.Domain.csproj", "GymApi/GymApi.Domain/"]
COPY ["GymApi/GymApi.Infrastructure/GymApi.Infrastructure.csproj", "GymApi/GymApi.Infrastructure/"]

RUN dotnet restore "GymApi/GymApi/GymApi.csproj"

COPY . .
WORKDIR "/src/GymApi/GymApi"
RUN dotnet publish "GymApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-10000} dotnet GymApi.dll"]
