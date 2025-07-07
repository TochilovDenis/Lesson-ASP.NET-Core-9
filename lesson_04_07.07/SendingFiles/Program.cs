var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) => 
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("html/index.html");
});

app.Run();