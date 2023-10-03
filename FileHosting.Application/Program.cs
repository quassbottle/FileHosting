using FileHosting.DataAccess.Repositories;
using FileHosting.Domain.Entities;
using FileHosting.Extensions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")!;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<NpgsqlRepository<DbFileMeta>>(repo =>
    ActivatorUtilities.CreateInstance<FileMetaRepository>(repo, connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MigrateDatabase<Program>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();