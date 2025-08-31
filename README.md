💳 BankService API

Учебный проект бэкенд-сервиса для банка.  
Реализован на **ASP.NET Core 8**, **Entity Framework Core** и **PostgreSQL**.  

Проект показывает основы построения REST API для банковских приложений:
- управление пользователями
- создание и работа со счетами 
- переводы между счетами 
- хранение данных в базе PostgreSQL через EF Core (Code-First + миграции)  


🚀 Технологии
- [.NET 8] — Web API  
- [Entity Framework Core] — ORM  
- [PostgreSQL] — база данных  
- [Swagger] — документация и тестирование API  


📂 Структура проекта

BankService/

 ├── Controllers/       # API контроллеры (Users, Accounts)
 
 ├── Data/              # DbContext (BankDbContext)
 
 ├── Dtos/              # Data Transfer Objects
 
 ├── Models/            # Сущности (User, Account)
 
 ├── Migrations/        # EF Core миграции
 
 ├── appsettings.json   # конфигурация
 
 └── Program.cs         # точка входа


📊 Настройка базы данных PostgreSQL
Подключиться к PostgreSQL и выполнить:

CREATE DATABASE "BankServiceDB";
CREATE USER bankpeople WITH PASSWORD 'bankpass';
GRANT ALL PRIVILEGES ON DATABASE "BankServiceDB" TO bankpeople;

Применить миграции
  dotnet ef database update
Запустить проект
  dotnet run
API будет доступно по адресу:
  https://localhost:7096/swagger/index.html

📌 Примеры API-запросов
Создать пользователя
POST /api/users
Content-Type: application/json
{
  "name": "Ivan",
  "email": "ivan@example.com"
}

Получить всех пользователей
GET /api/users

Создать счёт
POST /api/accounts
Content-Type: application/json
{
  "userId": 1,
  "initialBalance": 5000
}

Перевод между счетами
POST /api/accounts/transfer
Content-Type: application/json
{
  "fromAccountId": 1,
  "toAccountId": 2,
  "amount": 1000
}

👨‍💻 Автор Коваль Иван
Разработано для подготовки к собеседованию на позицию **Backend Developer (C#/.NET)**. 
