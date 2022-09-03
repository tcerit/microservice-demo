FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Products/Products.MessageRelay/Products.MessageRelay.csproj", "Services/Products/Products.MessageRelay/"]
COPY ["Services/Products/Products.Data/Products.Data.csproj", "Services/Products/Products.Data/"]
COPY ["Services/Products/Products.Domain/Products.Domain.csproj", "Services/Products/Products.Domain/"]
COPY ["MicroserviceDemo.Core/Core.csproj", "MicroserviceDemo.Core/"]
RUN dotnet restore "Services/Products/Products.MessageRelay/Products.MessageRelay.csproj"
COPY . .
WORKDIR "/src/Services/Products/Products.MessageRelay"
RUN dotnet build "Products.MessageRelay.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Products.MessageRelay.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.MessageRelay.dll"]
