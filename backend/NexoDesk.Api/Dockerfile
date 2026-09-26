FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/NexoDesk.Domain/NexoDesk.Domain.csproj backend/NexoDesk.Domain/
COPY backend/NexoDesk.Application/NexoDesk.Application.csproj backend/NexoDesk.Application/
COPY backend/NexoDesk.Infrastructure/NexoDesk.Infrastructure.csproj backend/NexoDesk.Infrastructure/
COPY backend/NexoDesk.Api/NexoDesk.Api.csproj backend/NexoDesk.Api/
RUN dotnet restore backend/NexoDesk.Api/NexoDesk.Api.csproj

COPY backend/ backend/
RUN dotnet publish backend/NexoDesk.Api/NexoDesk.Api.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "NexoDesk.Api.dll"]
