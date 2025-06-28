FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только файлы проектов
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["Core/Core.csproj", "Core/"]

# Восстанавливаем зависимости
RUN dotnet restore "WebAPI/WebAPI.csproj"

# Копируем весь исходный код
COPY . .

# Собираем приложение
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]