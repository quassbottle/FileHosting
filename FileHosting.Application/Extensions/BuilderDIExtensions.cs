using FileHosting.DataAccess.Providers;
using FileHosting.DataAccess.Repositories;
using FileHosting.DataAccess.Repositories.Interfaces;
using FileHosting.Domain.Services;
using FileHosting.Domain.Services.Interfaces;

namespace FileHosting.Extensions;

public static class BuilderDIExtensions
{
    public static void LoadDependencies(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")!;
        
        // database provider
        builder.Services.AddScoped<NpgsqlDataSourceProvider>(provider =>
            ActivatorUtilities.CreateInstance<NpgsqlDataSourceProvider>(provider, connectionString));
        
        LoadRepositories(builder);
        LoadServices(builder);
    }

    private static void LoadRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileDataRepository, FileDataRepository>();
        builder.Services.AddScoped<IFileMetaRepository, FileMetaRepository>();
        builder.Services.AddScoped<IFileUrlRepository, FileUrlRepository>();
    }

    private static void LoadServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IFileUploadService, FileUploadService>();
    }
}