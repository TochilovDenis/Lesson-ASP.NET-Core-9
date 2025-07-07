var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) => 
{
    var path = context.Request.Path;
    var fullPath = $"html/{path}";
    var response = context.Response;

    response.ContentType = "text/html; charset=utf-8";
    if(path == "/img")
    {
        await response.SendFileAsync("i.jpg");
    }
    else if (File.Exists(fullPath))
    {
        await response.SendFileAsync(fullPath);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsync("<h2>Not Found</h2>");
    }
});

app.Run();