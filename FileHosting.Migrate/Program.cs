using System.Reflection;
using DbUp;

namespace FileHosting.Migrate;

public class Program
{
    static int Main(string[] args)
    {
        var connectionString = args.FirstOrDefault() ??
                               "Host=localhost;User Id=filemanager;Password=password;Database=filemanager;Port=5432";
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrade = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrade.PerformUpgrade();
        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            return -1;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(value: "Success!");
        Console.ResetColor();
        return 0;
    }
}