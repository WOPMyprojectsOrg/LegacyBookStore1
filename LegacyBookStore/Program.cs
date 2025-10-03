using LegacyBookStore.Data;
using LegacyBookStore.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRepositories();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.Use(async (context, next) =>
{
    var userIdHeader = context.Request.Headers["X-User-Id"].ToString();
    if (!string.IsNullOrEmpty(userIdHeader) && int.TryParse(userIdHeader, out var userId))
    {
        // Нет проверки существования пользователя!
        context.Items["UserId"] = userId;
    }
    await next();
});

app.MapControllers();

app.Run();
