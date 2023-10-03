using FileHosting.DataAccess.Repositories;
using FileHosting.DataAccess.Repositories.Interfaces;
using FileHosting.Domain.Entities;
using FileHosting.Domain.Services;
using FileHosting.Domain.Services.Interfaces;

namespace FileHosting.Extensions;

public static class BuilderDiExtensions
{
    public static void LoadDependencies(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")!;

        LoadRepositories(builder, connectionString);
        LoadServices(builder);
    }

    private static void LoadRepositories(WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddScoped<NpgsqlRepository<DbFileMeta>>(repo =>
            ActivatorUtilities.CreateInstance<FileMetaRepository>(repo, connectionString));
        
        builder.Services.AddScoped<IFileDataRepository>(repo =>
            ActivatorUtilities.CreateInstance<FileDataRepository>(repo, connectionString));
    }

    private static void LoadServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileUploadService, FileUploadService>();
    }
}