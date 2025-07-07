var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) => await context.Response.SendFileAsync("C:\\Users\\dex\\Pictures\\Saved Pictures\\i.jpg"));

app.Run();