namespace IWebHostEnvironment
{
    public class UploadsMiddleware
    {
        private readonly RequestDelegate _next;
        public UploadsMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var request = context.Request;
                var path = request.Path;

                if (request.Path == "/upload" && request.Method == "POST")
                {
                    Console.WriteLine("start /upload");
                    await HandleUploadAsync(context);
                }
                else if (request.Path == "/image" && request.Method == "GET")
                {
                    Console.WriteLine("start /image");
                    await HandleImageAsync(context);
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
            catch (Exception)
            {
                context.Response.ContentType = "application/json";
                throw;
            }
        }

        async Task HandleUploadAsync(HttpContext context)
        {
            if (!context.Request.HasFormContentType)
            {
                throw new ArgumentException("Ожидается форма с файлами");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            Directory.CreateDirectory(uploadPath);

            foreach (var file in context.Request.Form.Files)
            {
                var fullPath = Path.Combine(uploadPath, file.FileName);
                using var fileStream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }

            await context.Response.WriteAsJsonAsync("Файлы успешно загружены");
        }

        async Task HandleImageAsync(HttpContext context)
        {
            var fileName = context.Request.Query["file_name"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Не указано имя файла");
            }

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var filePath = Path.Combine(uploadsDir, fileName);

            if (!filePath.StartsWith(uploadsDir))
            {
                throw new ForbiddenException("Доступ запрещен");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден");
            }

            context.Response.ContentType = GetContentType(filePath);
            await context.Response.SendFileAsync(filePath);
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

    }
}

