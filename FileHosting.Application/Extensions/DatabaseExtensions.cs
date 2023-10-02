using System.Reflection;
using DbUp;

namespace FileHosting.Extensions;

public static class DatabaseExtensions
{
    public static void MigrateDatabase<TContext>(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();

        logger.LogInformation("Migrating PostgreSQL database...");

        string connection = configuration.GetValue<string>("DatabaseSettings:ConnectionString")!;

        EnsureDatabase.For.PostgresqlDatabase(connection);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connection)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            logger.LogError(result.Error, "An error occurred while migrating the PostgreSQL database");
            return;
        }

        logger.LogInformation("PostgreSQL database migration has been completed");
    }
}