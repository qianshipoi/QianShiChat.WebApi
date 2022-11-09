#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/QianShiChat.Web.Entry/QianShiChat.Web.Entry.csproj", "src/QianShiChat.Web.Entry/"]
COPY ["src/QianShiChat.Web.Core/QianShiChat.Web.Core.csproj", "src/QianShiChat.Web.Core/"]
COPY ["src/QianShiChat.Application/QianShiChat.Application.csproj", "src/QianShiChat.Application/"]
COPY ["src/QianShiChat.Core/QianShiChat.Core.csproj", "src/QianShiChat.Core/"]
RUN dotnet restore "src/QianShiChat.Web.Entry/QianShiChat.Web.Entry.csproj"
COPY . .
WORKDIR "/src/src/QianShiChat.Web.Entry"
RUN dotnet build "QianShiChat.Web.Entry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QianShiChat.Web.Entry.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QianShiChat.Web.Entry.dll"]