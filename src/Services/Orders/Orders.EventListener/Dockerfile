FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Orders/Orders.EventListener/Orders.EventListener.csproj", "Services/Orders/Orders.EventListener/"]
COPY ["Services/Orders/Orders.Data/Orders.Data.csproj", "Services/Orders/Orders.Data/"]
COPY ["Services/Orders/Orders.Domain/Orders.Domain.csproj", "Services/Orders/Orders.Domain/"]
COPY ["MicroserviceDemo.Core/Core.csproj", "MicroserviceDemo.Core/"]
RUN dotnet restore "Services/Orders/Orders.EventListener/Orders.EventListener.csproj"
COPY . .
WORKDIR "/src/Services/Orders/Orders.EventListener"
RUN dotnet build "Orders.EventListener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orders.EventListener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.EventListener.dll"]