#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Api/WebApi/OnionSozluk.Api.WebApi/OnionSozluk.Api.WebApi.csproj", "src/Api/WebApi/OnionSozluk.Api.WebApi/"]
COPY ["src/Api/Infrastructure/OnionSozluk.Infrastructure.Persistence/OnionSozluk.Infrastructure.Persistence.csproj", "src/Api/Infrastructure/OnionSozluk.Infrastructure.Persistence/"]
COPY ["src/Api/Core/OnionSozluk.Api.Application/OnionSozluk.Api.Application.csproj", "src/Api/Core/OnionSozluk.Api.Application/"]
COPY ["src/Api/Core/OnionSozluk.Api.Domain/OnionSozluk.Api.Domain.csproj", "src/Api/Core/OnionSozluk.Api.Domain/"]
COPY ["src/Common/OnionSozluk.Common/OnionSozluk.Common.csproj", "src/Common/OnionSozluk.Common/"]
RUN dotnet restore "src/Api/WebApi/OnionSozluk.Api.WebApi/OnionSozluk.Api.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Api/WebApi/OnionSozluk.Api.WebApi"
RUN dotnet build "OnionSozluk.Api.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnionSozluk.Api.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnionSozluk.Api.WebApi.dll"]