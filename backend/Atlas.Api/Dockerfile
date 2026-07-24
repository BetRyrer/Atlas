FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Atlas.sln Directory.Build.props ./
COPY backend/Atlas.Domain/Atlas.Domain.csproj backend/Atlas.Domain/
COPY backend/Atlas.Application/Atlas.Application.csproj backend/Atlas.Application/
COPY backend/Atlas.Infrastructure/Atlas.Infrastructure.csproj backend/Atlas.Infrastructure/
COPY backend/Atlas.Api/Atlas.Api.csproj backend/Atlas.Api/
RUN dotnet restore backend/Atlas.Api/Atlas.Api.csproj

COPY backend/ backend/
RUN dotnet publish backend/Atlas.Api/Atlas.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/* \
    && groupadd --system --gid 1001 atlas \
    && useradd --system --uid 1001 --gid atlas atlas \
    && mkdir -p /app/data \
    && chown -R atlas:atlas /app

COPY --from=build /app/publish .

USER atlas
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Atlas.Api.dll"]
