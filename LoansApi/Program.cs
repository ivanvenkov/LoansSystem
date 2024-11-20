using LoansApi.DataAccess;
using LoansApi.DataAccess.Interfaces;
using LoansApi.Services;
using LoansApi.Services.Interfaces;
using Microsoft.Data.Sqlite;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
RegisterServices(builder.Services, connectionString);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    dataContext.InitializeDatabase();  // This initializes the DB (creates tables and seeds data)
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void RegisterServices(IServiceCollection services, string connectionString)
{
    services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));
    services.AddSingleton<DbInitializer>();
    services.AddScoped<ILoansRepository, LoansRepository>();
    services.AddScoped<ILoansService, LoansService>();
}

