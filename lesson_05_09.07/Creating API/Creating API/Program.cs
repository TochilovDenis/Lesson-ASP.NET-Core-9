using System.Text;

// начальные данные
List<Person> users = new List<Person>
{
    new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;
    
    if (path == "/api/users" && request.Method == "GET")
    {
        await response.WriteAsJsonAsync(users);
    }
    else if (path == "/api/post_user" && request.Method == "POST")
    {
        try
        {
            var user = await request.ReadFromJsonAsync<Person>();
            if (user != null)
            {
                user.Id = Guid.NewGuid().ToString();
                users.Add(user);
                await response.WriteAsJsonAsync(user);
            }
            else
            {
                throw new Exception("Некорректные данные");
            }
        }
        catch (Exception)
        {
            response.StatusCode = 400;
            await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
        }
    }
    else if (path == "/api/put_user" && request.Method == "PUT")
    {
        try
        {
            // получаем данные пользователя
            Person? userData = await request.ReadFromJsonAsync<Person>();
            if (userData != null)
            {
                // получаем пользователя по id
                var user = users.FirstOrDefault(u => u.Id == userData.Id);
                // если пользователь найден, изменяем его данные и отправляем обратно клиенту
                if (user != null)
                {
                    user.Id = userData.Id;
                    user.Age = userData.Age;
                    user.Name = userData.Name;
                    await response.WriteAsJsonAsync(user);
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
                }
            }
            else
            {
                throw new Exception("Некорректные данные");
            }
        }
        catch (Exception)
        {
            response.StatusCode = 400;
            await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
        }
    }
    else if (path.Value.StartsWith("/api/del_user/") && request.Method == "DELETE")
    {
        try
        {
            // Извлекаем ID из пути запроса
            var id = path.Value.Substring("/api/del_user/".Length);

            // Находим пользователя по ID
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                // Удаляем пользователя из списка
                users.Remove(user);

                // Возвращаем успешный ответ без тела
                response.StatusCode = 204;
                await response.WriteAsync("");
            }
            else
            {
                // Если пользователь не найден, возвращаем 404
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
            }
        }
        catch (Exception)
        {
            response.StatusCode = 400;
            await response.WriteAsJsonAsync(new { message = "Ошибка при удалении пользователя" });
        }
    }
    else if (request.Path == "/upload" && request.Method == "POST")
    {

        try
        {

            if (!request.HasFormContentType)
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync("Ожидается форма с файлами");
                return;
            }

            IFormFileCollection files = request.Form.Files;
            // путь к папке, где будут храниться файлы
            var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
            // создаем папку для хранения файлов
            Directory.CreateDirectory(uploadPath);

            foreach (var file in files)
            {
                // путь к папке uploads
                string fullPath = $"{uploadPath}/{file.FileName}";

                // сохраняем файл в папку uploads
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            await response.WriteAsJsonAsync("Файлы успешно загружены");
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            await response.WriteAsync($"Ошибка загрузки: {ex.Message}");
        }
    }
    else if (request.Path == "/image" && request.Method == "GET")
    {
        try
        {
            // Получаем имя файла из query-параметра
            string fileName = request.Query["file_name"].FirstOrDefault();

            // Проверяем, что имя файла указано
            if (string.IsNullOrWhiteSpace(fileName))
            {
                response.StatusCode = 400;
                await response.WriteAsJsonAsync("Не указано имя файла");
                return;
            }

            // Безопасное объединение путей
            string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            string filePath = Path.Combine(uploadsDir, fileName);

            // Защита от Directory Traversal атак
            if (!filePath.StartsWith(uploadsDir))
            {
                response.StatusCode = 403;
                await response.WriteAsJsonAsync("Доступ запрещен");
                return;
            }

            // Проверяем существование файла
            if (!File.Exists(filePath))
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync("Файл не найден");
                return;
            }

            // Определяем Content-Type по расширению файла
            string contentType = GetContentType(filePath);
            response.ContentType = contentType;

            // Отправляем файл
            await response.SendFileAsync(filePath);
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            await response.WriteAsJsonAsync($"Ошибка: {ex.Message}");
        }
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        var indexPath = "html/index.html";
        await response.SendFileAsync(indexPath);
    }

});


// Запускаем приложение с обработкой ошибок
try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Error server: {ex.Message}");
}

// Метод для определения Content-Type по расширению файла
static string GetContentType(string path)
{
    var types = new Dictionary<string, string>
    {
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".pdf", "application/pdf"},
        {".txt", "text/plain"},
        {".html", "text/html"},
        {".css", "text/css"},
        {".js", "application/javascript"}
    };

    string ext = Path.GetExtension(path).ToLowerInvariant();
    return types.TryGetValue(ext, out string type) ? type : "application/octet-stream";
}

public class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

