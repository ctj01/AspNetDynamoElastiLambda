FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN useradd -ms /bin/sh appuser
USER appuser
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AspNetElsDb/AspNetElsDb.csproj", "AspNetElsDb/"]
RUN dotnet restore "AspNetElsDb/AspNetElsDb.csproj"
COPY . .
WORKDIR "/src/AspNetElsDb"
RUN dotnet build "AspNetElsDb.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AspNetElsDb.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetElsDb.dll"]
