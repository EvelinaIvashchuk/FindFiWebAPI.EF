# FindFi.Ef — EF Core Code‑First Vertical Slice (Parallel Solution)

Це окремий solution на базі Entity Framework Core (Code‑First), який повторює предметну область попередніх завдань (квартири/оренда: Customer, Address, Product, Order, OrderItem, OrderDetails) і демонструє:
- Моделі домену (Entity) з навігаціями;
- Fluent API конфігурації (ключі, зовнішні ключі, унікальні обмеження, індекси, типи даних, check‑обмеження, computed‑колонки);
- DbContext з централізованим підключенням конфігурацій;
- Design‑time фабрику для міграцій (dotnet‑ef);
- Окремий Web/API (поки без контролерів — частина 1 вимагає саме моделювання + міграції).

## Структура
- FindFi.Ef.Domain — доменні сутності (Customer, CustomerAddress, Product, Order, OrderItem, OrderDetails)
- FindFi.Ef.Data — AppDbContext + Fluent API конфігурації + DesignTimeDbContextFactory
- FindFi.Ef.Bll — заготовка для майбутньої бізнес‑логіки (буде в наступних частинах)
- FindFi.Ef.Api — мінімальний Web/API з реєстрацією DbContext та Swagger
- FindFi.Ef.sln — окремий solution для EF‑версії

## Підключення до БД
Використовується MySQL/MariaDB через Pomelo.EntityFrameworkCore.MySql.

Параметри підключення читаються у такому порядку:
1) ConnectionStrings:DB1 (appsettings або змінна оточення ConnectionStrings__DB1)
2) ConnectionStrings:Default (або змінна ConnectionStrings__Default)

Швидкий приклад рядка підключення (локально):
```
Server=localhost;Port=3306;Database=app_ef;User ID=root;Password=12345678;SslMode=None;
```

- FindFi.Ef.Api/appsettings.json — містить приклад рядка підключення.
- FindFi.Ef.Data/appsettings.json — використовується інструментами міграцій (design‑time factory).

## Міграції (Code‑First)
Передумови:
- Встановіть dotnet‑ef (один раз глобально):
```
dotnet tool install --global dotnet-ef
```

Команди (в корені репозиторію або в каталозі FindFi.Ef.Data):

- Додавання початкової міграції:
```
dotnet ef migrations add InitialCreate --project FindFi.Ef.Data --startup-project FindFi.Ef.Api --output-dir Migrations
```
(Параметр --startup-project потрібен, якщо ви хочете використовувати конфігурацію Api; можна опустити, бо є DesignTimeDbContextFactory у Data.)

Альтернатива (через design‑time factory, достатньо перебувати в корені репо або у FindFi.Ef.Data):
```
dotnet ef migrations add InitialCreate --project FindFi.Ef.Data --output-dir Migrations
```

- Застосування міграцій у базу:
```
dotnet ef database update --project FindFi.Ef.Data
```

- Генерація SQL‑скрипту міграцій:
```
dotnet ef migrations script --project FindFi.Ef.Data -o ef-initial.sql
```

## Відповідність схемі
Fluent API відображає ключові вимоги з попередньої схеми:
- Customer: UQ Email, індекси по FullName/CreatedAt, default IsActive/Role, datetime(3) для CreatedAt.
- CustomerAddress: FK до Customer (cascade), індекс CustomerId, довжини рядків, default IsPrimary, datetime(3) з utc_timestamp(3).
- Product: UQ Code, індекси Name/IsActive, check Price>=0, decimal(12,2), datetime(3) з utc_timestamp(3).
- Order: FK до Customer (restrict), check TotalAmount>=0, індекси CustomerId/PlacedAt/Status, Currency як char(3) з default 'USD', CreatedAt як timestamp(3) з CURRENT_TIMESTAMP(3).
- OrderItem: UQ (OrderId, ProductId), FKs (Order cascade, Product restrict), computed LineTotal = UnitPrice*Quantity (stored), checks Quantity>0 і UnitPrice>=0, індекси по OrderId/ProductId.
- OrderDetails: 1–1 з Order (PK=FK OrderId), довжини рядків, cascade при видаленні замовлення.

## Запуск мінімального API
```
dotnet run -p FindFi.Ef.Api/FindFi.Ef.Api.csproj
```
Відкрийте Swagger UI: http://localhost:5000/swagger (порт залежить від запуску). Контролери будуть додані в наступних частинах завдання.

## Нотатки
- DbContext.OnModelCreating підключає всі конфігурації через ApplyConfigurationsFromAssembly — централізовано для всіх сутностей.
- DesignTimeDbContextFactory дозволяє запускати dotnet‑ef без окремого стартового проекту.
- Обмеження/дефолти/обчислювані колонки задаються з урахуванням можливостей MySQL (Pomelo).
