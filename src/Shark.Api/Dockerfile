#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Shark.Api/Shark.Api.csproj", "src/Shark.Api/"]
COPY ["src/Shark.Domain/Shark.Domain.csproj", "src/Shark.Domain/"]
COPY ["src/Shark.Application/Shark.Application.csproj", "src/Shark.Application/"]
COPY ["src/Shark.Infra/Shark.Infra.csproj", "src/Shark.Infra/"]
RUN dotnet restore "./src/Shark.Api/./Shark.Api.csproj"
COPY . .
WORKDIR "/src/src/Shark.Api"
RUN dotnet build "./Shark.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Shark.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shark.Api.dll"]