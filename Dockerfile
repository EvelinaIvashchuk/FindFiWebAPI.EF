# Сценарій збірки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копіюємо файли проектів та відновлюємо залежності
COPY ["FindFi.Ef.Api/FindFi.Ef.Api.csproj", "FindFi.Ef.Api/"]
COPY ["FindFi.Ef.Bll/FindFi.Ef.Bll.csproj", "FindFi.Ef.Bll/"]
COPY ["FindFi.Ef.Data/FindFi.Ef.Data.csproj", "FindFi.Ef.Data/"]
COPY ["FindFi.Ef.Domain/FindFi.Ef.Domain.csproj", "FindFi.Ef.Domain/"]

RUN dotnet restore "FindFi.Ef.Api/FindFi.Ef.Api.csproj"

# Копіюємо всі інші файли та збираємо проект
COPY . .
WORKDIR "/src/FindFi.Ef.Api"
RUN dotnet build "FindFi.Ef.Api.csproj" -c Release -o /app/build

# Публікація
FROM build AS publish
RUN dotnet publish "FindFi.Ef.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Фінальний образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Налаштування порту (за замовчуванням ASP.NET Core використовує 8080 у .NET 8+)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FindFi.Ef.Api.dll"]
