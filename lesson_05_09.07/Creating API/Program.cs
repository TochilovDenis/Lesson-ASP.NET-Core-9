using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
    //string expressionForNumber = "^/api/users/([0-9]+)$";   // если id представляет число

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
        catch (Exception ex)
        {
            response.StatusCode = 400;
            await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
        }
    }
    else if (path == "/api/users" && request.Method == "PUT")
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
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();


public class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}