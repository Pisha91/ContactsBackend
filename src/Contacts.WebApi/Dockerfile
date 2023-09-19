FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Contacts.WebApi/Contacts.WebApi.csproj", "Contacts.WebApi/"]
RUN dotnet restore "Contacts.WebApi/Contacts.WebApi.csproj"
COPY . .
WORKDIR "/src/Contacts.WebApi"
RUN dotnet build "Contacts.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contacts.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contacts.WebApi.dll"]
