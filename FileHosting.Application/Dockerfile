FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FileHosting.Application/FileHosting.Application.csproj", "FileHosting.Application/"]
RUN dotnet restore "FileHosting.Application/FileHosting.Application.csproj"
COPY . .
WORKDIR "/src/FileHosting.Application"
RUN dotnet build "FileHosting.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileHosting.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileHosting.Application.dll"]
