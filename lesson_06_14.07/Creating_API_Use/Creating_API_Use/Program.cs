using Creating_API_Use;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<UsersMiddleware>();

app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var indexPath = Path.Combine(Directory.GetCurrentDirectory(), "html/index.html");
    await context.Response.SendFileAsync(indexPath);
});

app.Run();