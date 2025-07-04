var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var time = DateTime.Now;

app.MapGet("/", () => time);

app.Run();
