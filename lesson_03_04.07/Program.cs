var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.UseWelcomePage();

app.Run(async (context) => await context.Response.WriteAsync("Hello METANIT.COM"));

app.Run();
