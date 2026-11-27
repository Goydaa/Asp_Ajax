# WebApplication3 — учебный проект «Магазин»

ASP.NET Core 9 (MVC + REST API), слои: Domain / Application (use cases) / Infrastructure (EF Core + SQLite) / Api / Web.

## Запуск
1. Нужен .NET 9 SDK (`dotnet --version`).
2. В папке проекта:
   ```
   dotnet restore
   dotnet run --launch-profile http
   ```
   или откройте `WebApplication3.sln` в Visual Studio и нажмите F5.
3. Откройте http://localhost:5208

База данных (SQLite, файл `app.db`) создаётся автоматически при первом запуске и заполняется тестовыми данными:
- клиенты: `user@example.com`, `blocked@example.com` (заблокирован)
- товары: Ноутбук, Мышь, Клавиатура, Монитор

## Что можно проверить
- Web: `/Orders/Create` — создать заказ → страница заказа → отмена заказа (товары возвращаются на склад).
- API:
  - `POST /api/orders` `{"customerEmail":"user@example.com","items":[{"productId":"<guid>","quantity":1}]}`
  - `GET /api/orders/{id}`
  - `POST /api/orders/{id}/cancel`
