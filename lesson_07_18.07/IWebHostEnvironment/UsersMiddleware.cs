using System.IO;

namespace IWebHostEnvironment
{
    public class UsersMiddleware
    {
        private readonly RequestDelegate _next;

        // начальные данные
        List<Person> _users = new List<Person>
        {
        new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
        new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
        new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
        };

        public UsersMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var request = context.Request;
                var path = request.Path;

                if (path == "/api/users" && request.Method == "GET")
                {
                    Console.WriteLine("start /api/users");
                    await HandleUsersGetAsync(context);
                }
                else if (path == "/api/post_user" && request.Method == "POST")
                {
                    Console.WriteLine("start /api/post_user");
                    await HandleUsersPostAsync(context);
                }
                else if (path == "/api/put_user" && request.Method == "PUT")
                {
                    Console.WriteLine("start /api/put_user");
                    await HandleUsersPutAsync(context);
                }
                else if (path.Value.StartsWith("/api/del_user/") && request.Method == "DELETE")
                {
                    Console.WriteLine("start /api/del_user/r");
                    await HandleUsersDeleteAsync(context);
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                throw;
            }
        }
        async Task HandleUsersGetAsync(HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(_users);
        }

        async Task HandleUsersPostAsync(HttpContext context)
        {
            var user = await context.Request.ReadFromJsonAsync<Person>();
            if (user == null)
            {
                throw new ArgumentException("Некорректные данные пользователя");
            }

            user.Id = Guid.NewGuid().ToString();
            _users.Add(user);
            await context.Response.WriteAsJsonAsync(user);
        }

        async Task HandleUsersPutAsync(HttpContext context)
        {
            var userData = await context.Request.ReadFromJsonAsync<Person>();
            if (userData == null)
            {
                throw new ArgumentException("Некорректные данные пользователя");
            }

            var user = _users.FirstOrDefault(u => u.Id == userData.Id);
            if (user == null)
            {
                throw new NotFoundException("Пользователь не найден");
            }

            user.Age = userData.Age;
            user.Name = userData.Name;
            await context.Response.WriteAsJsonAsync(user);
        }

        async Task HandleUsersDeleteAsync(HttpContext context)
        {
            var id = context.Request.Path.Value?["/api/del_user/".Length..];
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Пользователь не найден");
            }

            _users.Remove(user);
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            await context.Response.WriteAsJsonAsync("Пользователь удален");
        }

    }
    public class Person
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }
}
