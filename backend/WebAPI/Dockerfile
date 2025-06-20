FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "WebAPI/WebAPI.csproj"
COPY . .
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]