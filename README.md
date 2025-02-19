# Спринты нейроинтеграции - backend
Приложение предназначено для упрощения прохождения спринтов нейроинтеграции.

## Возможности

- Регистрация и вход пользователей.
- Авторизация через Telegram.
- Работа с API для управления спринтами, проектами и задачами.
- Для тестирования API доступен Swagger.

## Установка и запуск

- Должны быть установлены Docker и Docker Compose (для этого можно просто установить Docker Desktop).
- Файл `.env` для Docker Compose. Этот файл содержит секреты, необходимые для запуска приложения, и его нужно запросить у меня.

### Шаги для запуска приложения

1. Склонируйте репозиторий на компьютер:

   ```bash
   git clone https://github.com/neurointegration/backend-neurointegration-sprints
   cd backend-neurointegration-sprints
   ```

2. Запросите файл `.env` и разместите его в корневой папке проекта.

3. Запустите приложение с помощью Docker Compose:

   ```bash
   docker-compose up -d
   ```

4. Swagger-документация будет доступна по адресу:
   [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)

### Остановка приложения

Для остановки всех контейнеров выполните команду:

```bash
docker-compose down
```

### Пересборка приложения

Если вы внесли изменения в код приложения и хотите пересобрать контейнеры, выполните:

```bash
docker-compose build
```

Затем снова запустите контейнеры:

```bash
docker-compose up -d
```

## Тестовые данные

Для тестирования приложения доступен пользователь:

- **Email:** client
- **Пароль:** Client1!

## Особенности авторизации через Telegram

1. Пример реализации авторизации через Telegram доступен в файле:
   `src/Api/Controllers/TelegramTestController.cs`.

2. Настройка внешнего  вида виджета авторизации Telegram:
   [Telegram Widget](https://core.telegram.org/widgets/login).

3. Для корректной локальной работы Telegram-авторизации:

   - Приложение должно быть запущено на порту `80` для HTTP или `443` для HTTPS.
   - Открывайте виджет по адресу `http://127.0.0.1` или `https://127.0.0.1`. На других портах или по адресу `localhost` виджет работать не будет.

## Примечания по API

- В приложении реализованы основные функции для работы со спринтами, проектами и задачами.
- При создании и обновлении сущностей многие поля являются необязательными, их можно пропускать.

---
