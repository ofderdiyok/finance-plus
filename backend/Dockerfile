# SDK build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./FinancePlus.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish "FinancePlus.csproj" -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "FinancePlus.dll"]
