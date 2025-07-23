using IWebHostEnvironment;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

 
if (app.Environment.IsDevelopment()) // http
{
   //app.Run(async (context) => await context.Response.WriteAsync("In Development Stage"));
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMainMiddleware();

}
else if (app.Environment.IsProduction()) // https
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMainMiddleware();
}
else if (app.Environment.IsEnvironment("test_user")) // если проект в состоянии тестирования пользователя
{
    app.UseUsersMiddleware();
}
else if (app.Environment.IsEnvironment("test_uploads")) // если проект в состоянии тестирования загрузка файла
{
    app.UseUploadsMiddleware(); 
}
else
{
    app.Run(async (context) => await context.Response.WriteAsync("In Production Stage"));
}
Console.WriteLine($"{app.Environment.EnvironmentName}");

app.Run();


/*
Практическое задание, создать несколько сред
+ 1 Тестирование /api/users
+ 2 Тестирование загрузки файлов
+ 3 Все приложение вместе на стадии Разработки  http://
+ 4 Все приложение вместе на стадии Production https://
*/