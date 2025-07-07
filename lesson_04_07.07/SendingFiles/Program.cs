var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) => 
{
    var path = context.Request.Path;
    var fullPath = $"html/{path}";
    var response = context.Response;

    
    if(path == "/img")
    {
        response.ContentType = "image/jpg; charset=utf-8";
        await response.SendFileAsync("i.jpg");
    }
    else if (File.Exists(fullPath))
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync(fullPath);
    }
    else if (path == "/dl")
    {
        //save jpg
        response.Headers.ContentDisposition = "attachment; filename=my_i.jpg";
        await response.SendFileAsync("i.jpg");
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        response.StatusCode = 404;
        await response.WriteAsync("<h2>Not Found</h2>");
    }
});

app.Run();