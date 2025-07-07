var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    /*
    Свойство QueryString позволяет получить строку запроса. Строка запроса представляет
    ту часть запрошенного адреса, которая идет после символа ? и представляет набор параметров,
    разделенных символом амперсанда &:
     
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync($"<p>Path: {context.Request.Path}</p>" +
        $"<p>QueryString: {context.Request.QueryString}</p>"); 
    
    В данном случае идет обращение по адресу https://localhost:7256/users?name=Tom&age=37
     */

    /*
    С помощью свойства Query можно получить все параметры строки запроса в виде словаря:
     
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<h3>Параметры строки запроса</h3><table>");
    stringBuilder.Append("<tr><td>Параметр</td><td>Значение</td></tr>");
    foreach (var param in context.Request.Query)
    {
        stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
    }
    stringBuilder.Append("</table>");
    await context.Response.WriteAsync(stringBuilder.ToString());
    
     */

    /*
    Соответственно можно вытащить из словаря Query значения отдельных параметров:
    */
    context.Response.ContentType = "text/html; charset=utf-8";
    string name = context.Request.Query["name"];
    string age = context.Request.Query["age"];
    string city = context.Request.Query["city"];
    await context.Response.WriteAsync($"{name} - {age} -{city}");
});

app.Run();