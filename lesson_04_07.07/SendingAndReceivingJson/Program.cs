var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    //Person tom = new("Tom", 22);
    //await context.Response.WriteAsJsonAsync(tom);

    //var response = context.Response;
    //response.Headers.ContentType = "application/json; charset=utf-8";
    //await response.WriteAsync("{\"name\":\"Tom\",\"age\":37}");

    var response = context.Response;
    var request = context.Request;
    if (request.Path == "/api/user")
    {
        var message = "Некорректные данные";   // содержание сообщения по умолчанию
        try
        {
            // пытаемся получить данные json
            var person = await request.ReadFromJsonAsync<Person>();
            if (person != null) // если данные сконвертированы в Person
                message = $"Name: {person.Name}  Age: {person.Age}";
        }
        catch { }
        // отправляем пользователю данные
        await response.WriteAsJsonAsync(new { text = message });
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();

public record Person(string Name, int Age);