using System.Data;
using FileHosting.DataAccess.Entities;
using FileHosting.DataAccess.Extensions;
using FileHosting.DataAccess.Providers;
using FileHosting.DataAccess.Repositories.Interfaces;
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
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileData>();
        return result.First();
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
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileData>();
        return result.First();
    }

    public async Task<DbFileData> CreateAsync(DbFileData data)
    {
        var cmd = _dataSource.CreateCommand("INSERT INTO file_data (data, meta_id) VALUES (@data, @meta_id) RETURNING *;");
        
        cmd.Parameters.AddWithValue("data", NpgsqlDbType.Bytea, data.Data);
        cmd.Parameters.AddWithValue("meta_id", data.FileMetaId);
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileData>();
        return result.First();
    }

    public async Task<List<DbFileData>> GetAllAsync()
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_data");
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileData>();
        return result;
    }
}