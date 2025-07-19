using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Creating_API_Use
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next; // Следующий middleware в цепочке
            _logger = logger; // Логгер для записи ошибок
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Пробуем выполнить следующий middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Обрабатываем исключение
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json"; // Устанавливаем тип контента
            var response = context.Response;

            // Определяем код статуса в зависимости от типа исключения
            switch (exception)
            {
                case ArgumentException _:
                    // Для ошибок клиента (некорректные данные и т.д.)
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
                case FileNotFoundException _:
                    // Для ошибок файлов (некорректные данные и т.д.)
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;   // 404
                    break;
                case UnauthorizedAccessException _:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;  // 403
                    break;
                default:
                    // Для всех остальных ошибок
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;  // 500
                    break;
            }

            // Логируем ошибку
            _logger.LogError(exception, "Произошла ошибка при обработке запроса");

            // Формируем ответ клиенту
            //await context.Response.WriteAsJsonAsync(new
            //{
            //    response.StatusCode,
            //    exception.Message
            //});

            await context.Response.WriteAsync($"{exception.Message}, {response.StatusCode}");
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
