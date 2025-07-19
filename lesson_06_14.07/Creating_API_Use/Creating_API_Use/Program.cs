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
app.UseMainMiddleware();

app.Run();