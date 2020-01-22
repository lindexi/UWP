FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BaqulukaNercerewhelbeba.csproj", ""]
RUN dotnet restore "./BaqulukaNercerewhelbeba.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BaqulukaNercerewhelbeba.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaqulukaNercerewhelbeba.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaqulukaNercerewhelbeba.dll"]