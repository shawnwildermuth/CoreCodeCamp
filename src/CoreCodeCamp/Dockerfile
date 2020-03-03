#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs
WORKDIR /src
COPY ["CoreCodeCamp/CoreCodeCamp.csproj", "CoreCodeCamp/"]
RUN dotnet restore "CoreCodeCamp/CoreCodeCamp.csproj"
COPY . .
WORKDIR "/src/CoreCodeCamp"
RUN dotnet build "CoreCodeCamp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreCodeCamp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreCodeCamp.dll"]