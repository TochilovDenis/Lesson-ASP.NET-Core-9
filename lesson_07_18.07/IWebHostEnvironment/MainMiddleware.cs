namespace Creating_API_Use
{
    public class MainMiddleware
    {
        private readonly RequestDelegate _next;

        public MainMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Method MainMiddleware");

            if (context.Request.Path == "/index" && context.Request.Method == "GET")
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                var indexPath = Path.Combine(Directory.GetCurrentDirectory(), "html/index.html");
                await context.Response.SendFileAsync(indexPath);
            }
            else
            {
                Console.WriteLine("Страница не найдена 404");
                throw new FileNotFoundException("Страница не найдена");
            }
        }
    }
}
