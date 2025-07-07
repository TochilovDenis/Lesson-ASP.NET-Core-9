var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) => await context.Response.SendFileAsync("i.jpg"));

app.Run();