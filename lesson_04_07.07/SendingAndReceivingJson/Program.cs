var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    Person tom = new("Tom", 22);
    await context.Response.WriteAsJsonAsync(tom);

    //var response = context.Response;
    //response.Headers.ContentType = "application/json; charset=utf-8";
    //await response.WriteAsync("{\"name\":\"Tom\",\"age\":37}");
});

app.Run();

public record Person(string Name, int Age);