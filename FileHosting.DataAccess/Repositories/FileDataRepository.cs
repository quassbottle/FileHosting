using FileHosting.DataAccess.Providers;
using FileHosting.DataAccess.Repositories.Interfaces;
using FileHosting.Domain.Entities;
using Npgsql;
using NpgsqlTypes;

namespace FileHosting.DataAccess.Repositories;

public class FileDataRepository : IFileDataRepository
{
    private readonly NpgsqlDataSource _dataSource;
    
    public FileDataRepository(NpgsqlDataSourceProvider provider)
    {
        _dataSource = provider.GetDataSource();
    }
    
    public async Task<DbFileData> UpdateAsync(DbFileData data, Guid id)
    {
        var cmd = _dataSource.CreateCommand("UPDATE file_data SET data = @data, meta_id = @meta_id WHERE id = @id RETURNING *;");

        cmd.Parameters.AddWithValue("data", NpgsqlDbType.Bytea, data.Data);
        cmd.Parameters.AddWithValue("meta_id", data.FileMetaId);  
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var fileData = reader.GetFieldValueAsync<byte[]>(1);
        var metaId = reader.GetFieldValueAsync<Guid>(2);
        
        Task.WaitAll(guid, fileData, metaId);
        
        return new DbFileData()
        {
            Id = guid.Result,
            FileMetaId = metaId.Result,
            Data = fileData.Result
        };
    }

    public async Task<int> DeleteByGuid(Guid id)
    {
        var cmd = _dataSource.CreateCommand("DELETE FROM file_data WHERE id = $1;");
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<DbFileData> FindByGuidAsync(Guid id)
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_data WHERE id = $1");
        
        cmd.Parameters.AddWithValue(id);
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var fileData = reader.GetFieldValueAsync<byte[]>(1);
        var metaId = reader.GetFieldValueAsync<Guid>(2);
        
        Task.WaitAll(guid, fileData, metaId);
        
        return new DbFileData()
        {
            Id = guid.Result,
            FileMetaId = metaId.Result,
            Data = fileData.Result
        };
    }

    public async Task<DbFileData> CreateAsync(DbFileData data)
    {
        var cmd = _dataSource.CreateCommand("INSERT INTO file_data (data, meta_id) VALUES (@data, @meta_id) RETURNING *;");
        
        cmd.Parameters.AddWithValue("data", NpgsqlDbType.Bytea, data.Data);
        cmd.Parameters.AddWithValue("meta_id", data.FileMetaId);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var fileData = reader.GetFieldValueAsync<byte[]>(1);
        var metaId = reader.GetFieldValueAsync<Guid>(2);
        
        Task.WaitAll(guid, fileData, metaId);
        
        return new DbFileData()
        {
            Id = guid.Result,
            FileMetaId = metaId.Result,
            Data = fileData.Result
        };
    }

    public async Task<List<DbFileData>> GetAllAsync()
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_data");
        await using var reader = await cmd.ExecuteReaderAsync();

        var files = new List<DbFileData>();

        while (await reader.ReadAsync())
        {
            var guid = reader.GetFieldValueAsync<Guid>(0);
            var fileData = reader.GetFieldValueAsync<byte[]>(1);
            var metaId = reader.GetFieldValueAsync<Guid>(2);
        
            Task.WaitAll(guid, fileData, metaId);
        
            files.Add(new DbFileData
            {
                Id = guid.Result,
                FileMetaId = metaId.Result,
                Data = fileData.Result
            });
        }

        return files;
    }
}