using FileHosting.Domain.Entities;

namespace FileHosting.DataAccess.Repositories;

public class FileUrlRepository : NpgsqlRepository<DbFileUrl>
{
    public FileUrlRepository(string connectionString) : base(connectionString)
    {
    }

    public override async Task<DbFileUrl> UpdateAsync(DbFileUrl url, Guid id)
    {
        var cmd = await CreateCommandAsync("UPDATE file_url SET meta_id = @meta_id WHERE id = @id RETURNING *;");

        cmd.Parameters.AddWithValue("meta_id", url.FileMetaId);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var metaId = reader.GetFieldValueAsync<Guid>(1);
        
        Task.WaitAll(guid, metaId);
        
        return new DbFileUrl()
        {
            Guid = guid.Result,
            FileMetaId = metaId.Result
        };
    }

    public override async Task<int> DeleteByGuid(Guid id)
    {
        var cmd = await CreateCommandAsync("DELETE FROM file_url WHERE id = $1;");
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public override async Task<DbFileUrl> FindByGuidAsync(Guid id)
    {
        var cmd = await CreateCommandAsync("SELECT * FROM file_url WHERE id = $1");
        
        cmd.Parameters.AddWithValue(id);
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var metaId = reader.GetFieldValueAsync<Guid>(1);
        
        Task.WaitAll(guid, metaId);
        
        return new DbFileUrl()
        {
            Guid = guid.Result,
            FileMetaId = metaId.Result
        };
    }

    public override async Task<DbFileUrl> CreateAsync(DbFileUrl url)
    {
        var cmd = await CreateCommandAsync(
            "INSERT INTO file_url (id, meta_id) VALUES (DEFAULT, @meta_id) RETURNING *;");

        cmd.Parameters.AddWithValue("meta_id", url.FileMetaId);

        await using var reader = await cmd.ExecuteReaderAsync();

        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var metaId = reader.GetFieldValueAsync<Guid>(1);

        Task.WaitAll(guid, metaId);

        return new DbFileUrl()
        {
            Guid = guid.Result,
            FileMetaId = metaId.Result
        };
    }

    public override async Task<List<DbFileUrl>> GetAllAsync()
    {
        var cmd = await CreateCommandAsync("SELECT * FROM file_url");
        await using var reader = await cmd.ExecuteReaderAsync();

        var urls = new List<DbFileUrl>();

        while (await reader.ReadAsync())
        {
            var guid = reader.GetFieldValueAsync<Guid>(0);
            var metaId = reader.GetFieldValueAsync<Guid>(1);
        
            Task.WaitAll(guid, metaId);
        
            urls.Add(new DbFileUrl()
            {
                Guid = guid.Result,
                FileMetaId = metaId.Result
            });
        }

        return urls;
    }
}