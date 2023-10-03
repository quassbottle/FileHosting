using FileHosting.Domain.Entities;
using NpgsqlTypes;

namespace FileHosting.DataAccess.Repositories;

public class FileMetaRepository : NpgsqlRepository<FileMeta>
{
    public FileMetaRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<FileMeta> UpdateAsync(FileMeta meta, Guid id)
    {
        var cmd = await CreateCommandAsync("UPDATE file_meta SET size = @size, name = @name WHERE id = @id RETURNING *;");

        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("size", meta.Size ?? 0);
        cmd.Parameters.AddWithValue("name", meta.Name);   
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var size = reader.GetFieldValueAsync<long>(1);
        var name = reader.GetFieldValueAsync<string>(2);
        
        Task.WaitAll(guid, size, name);
        
        return new FileMeta
        {
            Guid = guid.Result,
            Name = name.Result,
            Size = size.Result
        };
    }

    public async Task<int> DeleteByGuid(Guid id)
    {
        var cmd = await CreateCommandAsync("DELETE FROM file_meta WHERE id = $1;");
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<FileMeta> FindByGuidAsync(Guid id)
    {
        var cmd = await this.CreateCommandAsync("SELECT * FROM file_meta WHERE id = $1");
        
        cmd.Parameters.AddWithValue(id);
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var size = reader.GetFieldValueAsync<long>(1);
        var name = reader.GetFieldValueAsync<string>(2);
        
        Task.WaitAll(guid, size, name);
        
        return new FileMeta
        {
            Guid = guid.Result,
            Name = name.Result,
            Size = size.Result
        };
    }

    public async Task<FileMeta> UpdateAsync(FileMeta meta)
    {
        var cmd = await CreateCommandAsync("INSERT INTO file_meta (size, name) VALUES (@size, @name) RETURNING *;");
        
        cmd.Parameters.AddWithValue("size", meta.Size ?? 0);
        cmd.Parameters.AddWithValue("name", meta.Name);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var size = reader.GetFieldValueAsync<long>(1);
        var name = reader.GetFieldValueAsync<string>(2);
        
        Task.WaitAll(guid, size, name);
        
        return new FileMeta
        {
            Guid = guid.Result,
            Name = name.Result,
            Size = size.Result
        };
    }

    public async Task<List<FileMeta>> GetAllAsync()
    {
        var cmd = await CreateCommandAsync("SELECT * FROM file_meta");
        await using var reader = await cmd.ExecuteReaderAsync();

        var meta = new List<FileMeta>();

        while (await reader.ReadAsync())
        {
            var guid = reader.GetFieldValueAsync<Guid>(0);
            var size = reader.GetFieldValueAsync<long>(1);
            var name = reader.GetFieldValueAsync<string>(2);
            
            Task.WaitAll(guid, size, name);
            
            meta.Add(new FileMeta
            {
                Guid = guid.Result,
                Name = name.Result,
                Size = size.Result
            });
        }

        return meta;
    }
}