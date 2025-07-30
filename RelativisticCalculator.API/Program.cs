using Microsoft.EntityFrameworkCore;
using RelativisticCalculator.API.Configuration;
using RelativisticCalculator.API.Data;
using RelativisticCalculator.API.Middleware;
using RelativisticCalculator.API.Services.Implementation;
using RelativisticCalculator.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStarService, StarService>();
builder.Services.AddScoped<IRelService, RelService>();
builder.Services.AddSingleton<IRelCalcService, RelCalcService>();

builder.Services.Configure<PhysicalConstants>(
    builder.Configuration.GetSection("PhysicalConstants"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseAuthorization();

app.MapControllers();

// Ensure the database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();