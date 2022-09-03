FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Customers/Customers.MessageRelay/Customers.MessageRelay.csproj", "Services/Customers/Customers.MessageRelay/"]
COPY ["Services/Customers/Customers.Data/Customers.Data.csproj", "Services/Customers/Customers.Data/"]
COPY ["Services/Customers/Customers.Domain/Customers.Domain.csproj", "Services/Customers/Customers.Domain/"]
COPY ["MicroserviceDemo.Core/Core.csproj", "MicroserviceDemo.Core/"]
RUN dotnet restore "Services/Customers/Customers.MessageRelay/Customers.MessageRelay.csproj"
COPY . .

WORKDIR "/src/Services/Customers/Customers.MessageRelay"
RUN dotnet build "Customers.MessageRelay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customers.MessageRelay.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customers.MessageRelay.dll"]