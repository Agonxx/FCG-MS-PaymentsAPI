FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["PaymentsAPI.Api/PaymentsAPI.Api.csproj", "PaymentsAPI.Api/"]
COPY ["PaymentsAPI.Application/PaymentsAPI.Application.csproj", "PaymentsAPI.Application/"]
COPY ["PaymentsAPI.Domain/PaymentsAPI.Domain.csproj", "PaymentsAPI.Domain/"]
COPY ["PaymentsAPI.Infrastructure/PaymentsAPI.Infrastructure.csproj", "PaymentsAPI.Infrastructure/"]
RUN dotnet restore "PaymentsAPI.Api/PaymentsAPI.Api.csproj"
COPY . .
WORKDIR "/src/PaymentsAPI.Api"
RUN dotnet build "PaymentsAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentsAPI.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentsAPI.Api.dll"]
