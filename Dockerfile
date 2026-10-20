FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Gymify.API/Gymify.API.csproj", "Gymify.API/"]
COPY ["src/Gymify.Application/Gymify.Application.csproj", "Gymify.Application/"]
COPY ["src/Gymify.Domain/Gymify.Domain.csproj", "Gymify.Domain/"]
COPY ["src/Gymify.Infrastructure/Gymify.Infrastructure.csproj", "Gymify.Infrastructure/"]

RUN dotnet restore "./Gymify.API/Gymify.API.csproj"

COPY src/ .
WORKDIR "/src/Gymify.API"
RUN dotnet build "Gymify.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gymify.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gymify.API.dll"]
