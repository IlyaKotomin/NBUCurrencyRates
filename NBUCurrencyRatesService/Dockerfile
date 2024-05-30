FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NBUCurrencyRates/NBUCurrencyRates.csproj", "NBUCurrencyRates/"]
RUN dotnet restore "NBUCurrencyRates/NBUCurrencyRates.csproj"
COPY . .
WORKDIR "/src/NBUCurrencyRates"
RUN dotnet build "NBUCurrencyRates.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NBUCurrencyRates.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NBUCurrencyRates.dll"]
