FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Atlas.sln Directory.Build.props ./
COPY src/Atlas.Domain/Atlas.Domain.csproj src/Atlas.Domain/
COPY src/Atlas.Application/Atlas.Application.csproj src/Atlas.Application/
COPY src/Atlas.Infrastructure/Atlas.Infrastructure.csproj src/Atlas.Infrastructure/
COPY src/Atlas.Api/Atlas.Api.csproj src/Atlas.Api/
RUN dotnet restore src/Atlas.Api/Atlas.Api.csproj

COPY src/ src/
RUN dotnet publish src/Atlas.Api/Atlas.Api.csproj -c Release -o /app/publish --no-restore

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
