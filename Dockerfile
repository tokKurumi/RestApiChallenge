FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY MultithredRest/MultithredRest.csproj .
RUN dotnet restore "MultithredRest.csproj"
COPY MultithredRest/ .
RUN dotnet publish "MultithredRest.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as final
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT [ "dotnet", "MultithredRest.dll" ]