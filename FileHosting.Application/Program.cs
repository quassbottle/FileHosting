using FileHosting.DataAccess.Repositories;
using FileHosting.Domain.Entities;
using FileHosting.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")!;

var test = new FileMetaRepository(connectionString);

Console.WriteLine(test.UpdateAsync(new FileMeta
{
    Size = 100,
    Name = "fff"
},Guid.Parse("3a02b2da-5648-4440-b3a9-e305d1fc641f")).Result.Guid);

return;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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