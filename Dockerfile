FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
EXPOSE 5024

WORKDIR /src
COPY ["UserAdministration/UserAdministration.csproj", "UserAdministration/"]
COPY ["UserAdministration.Infrastructure/UserAdministration.Infrastructure.csproj", "UserAdministration.Infrastructure/"]
COPY ["UserAdministration.Application/UserAdministration.Application.csproj", "UserAdministration.Application/"]
COPY ["UserAdministration.Core/UserAdministration.Core.csproj", "UserAdministration.Core/"]
COPY ["UserAdministration.Application.DTO/UserAdministration.Application.DTO.csproj", "UserAdministration.Application.DTO/"]
RUN dotnet restore "UserAdministration/UserAdministration.csproj"
COPY . .
WORKDIR "/src/UserAdministration"
RUN dotnet build "UserAdministration.csproj" -c Release -o /build

FROM build AS publish
RUN dotnet publish "UserAdministration.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "UserAdministration.dll"]