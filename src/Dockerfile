#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Moonglade.ContentSecurity.csproj", "."]
RUN dotnet restore "./Moonglade.ContentSecurity.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Moonglade.ContentSecurity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Moonglade.ContentSecurity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true