using FileHosting.DataAccess.Providers;
using FileHosting.DataAccess.Repositories.Interfaces;
using FileHosting.Domain.Entities;
using Npgsql;

namespace FileHosting.DataAccess.Repositories;

public class FileUrlRepository : IFileUrlRepository
{
    private readonly NpgsqlDataSource _dataSource;
    
    public FileUrlRepository(NpgsqlDataSourceProvider provider)
    {
        _dataSource = provider.GetDataSource();
    }

    public async Task<DbFileUrl> UpdateAsync(DbFileUrl url, Guid id)
    {
        var cmd = _dataSource.CreateCommand("UPDATE file_url SET meta_id = @meta_id WHERE id = @id RETURNING *;");

        cmd.Parameters.AddWithValue("meta_id", url.FileMetaId);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var metaId = reader.GetFieldValueAsync<Guid>(1);
        
        Task.WaitAll(guid, metaId);
        
        return new DbFileUrl()
        {
            Id = guid.Result,
            FileMetaId = metaId.Result
        };
    }

    public async Task<int> DeleteByGuid(Guid id)
    {
        var cmd = _dataSource.CreateCommand("DELETE FROM file_url WHERE id = $1;");
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<DbFileUrl> FindByGuidAsync(Guid id)
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_url WHERE id = $1");
        
        cmd.Parameters.AddWithValue(id);
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var metaId = reader.GetFieldValueAsync<Guid>(1);
        
        Task.WaitAll(guid, metaId);
        
        return new DbFileUrl()
        {
            Id = guid.Result,
            FileMetaId = metaId.Result
        };
    }

    public async Task<DbFileUrl> CreateAsync(DbFileUrl url)
    {
        var cmd = _dataSource.CreateCommand(
            "INSERT INTO file_url (id, meta_id) VALUES (DEFAULT, @meta_id) RETURNING *;");

        cmd.Parameters.AddWithValue("meta_id", url.FileMetaId);

        await using var reader = await cmd.ExecuteReaderAsync();

        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var metaId = reader.GetFieldValueAsync<Guid>(1);

        Task.WaitAll(guid, metaId);

        return new DbFileUrl()
        {
            Id = guid.Result,
            FileMetaId = metaId.Result
        };
    }

    public async Task<List<DbFileUrl>> GetAllAsync()
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_url");
        await using var reader = await cmd.ExecuteReaderAsync();

        var urls = new List<DbFileUrl>();

        while (await reader.ReadAsync())
        {
            var guid = reader.GetFieldValueAsync<Guid>(0);
            var metaId = reader.GetFieldValueAsync<Guid>(1);
        
            Task.WaitAll(guid, metaId);
        
            urls.Add(new DbFileUrl()
            {
                Id = guid.Result,
                FileMetaId = metaId.Result
            });
        }

        return urls;
    }
}