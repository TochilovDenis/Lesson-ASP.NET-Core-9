using Creating_API_Use;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();


app.UseUsersMiddleware();
app.UseMiddleware<UsersMiddleware>();


app.Run(async (context) =>
{
    Console.WriteLine("Метод run");

    var response = context.Response;
    var request = context.Request;

    response.ContentType = "text/html; charset=utf-8";
    var indexPath = "html/index.html";
    await response.SendFileAsync(indexPath);
});


// Запускаем приложение с обработкой ошибок
try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Error server: {ex.Message}");
}



