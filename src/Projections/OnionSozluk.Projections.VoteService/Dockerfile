#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Projections/OnionSozluk.Projections.VoteService/OnionSozluk.Projections.VoteService.csproj", "src/Projections/OnionSozluk.Projections.VoteService/"]
COPY ["src/Common/OnionSozluk.Common/OnionSozluk.Common.csproj", "src/Common/OnionSozluk.Common/"]
RUN dotnet restore "src/Projections/OnionSozluk.Projections.VoteService/OnionSozluk.Projections.VoteService.csproj"
COPY . .
WORKDIR "/src/src/Projections/OnionSozluk.Projections.VoteService"
RUN dotnet build "OnionSozluk.Projections.VoteService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnionSozluk.Projections.VoteService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnionSozluk.Projections.VoteService.dll"]