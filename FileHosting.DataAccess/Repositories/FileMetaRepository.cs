using System.Data;
using FileHosting.DataAccess.Entities;
using FileHosting.DataAccess.Extensions;
using FileHosting.DataAccess.Providers;
using FileHosting.DataAccess.Repositories.Interfaces;
using Npgsql;

namespace FileHosting.DataAccess.Repositories;

public class FileMetaRepository : IFileMetaRepository
{
    private readonly NpgsqlDataSource _dataSource;
    
    public FileMetaRepository(NpgsqlDataSourceProvider provider)
    {
        _dataSource = provider.GetDataSource();
    }

    public async Task<DbFileMeta> UpdateAsync(DbFileMeta meta, Guid id)
    {
        var cmd = _dataSource.CreateCommand("UPDATE file_meta SET size = @size, name = @name, type = @type WHERE id = @id RETURNING *;");

        cmd.Parameters.AddWithValue("size", meta.Size);
        cmd.Parameters.AddWithValue("name", meta.Name);   
        cmd.Parameters.AddWithValue("type", meta.Type);   
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileMeta>();
        return result.First();
    }

    public async Task<int> DeleteByGuid(Guid id)
    {
        var cmd = _dataSource.CreateCommand("DELETE FROM file_meta WHERE id = $1;");
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<DbFileMeta> FindByGuidAsync(Guid id)
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_meta WHERE id = $1");
        
        cmd.Parameters.AddWithValue(id);

        var result = await cmd.ExecuteAutoReaderAsync<DbFileMeta>();
        return result.First();
    }

    public async Task<DbFileMeta> CreateAsync(DbFileMeta meta)
    {
        var cmd = _dataSource.CreateCommand("INSERT INTO file_meta (size, name, type) VALUES (@size, @name, @type) RETURNING *;");
        
        cmd.Parameters.AddWithValue("size", meta.Size);
        cmd.Parameters.AddWithValue("name", meta.Name);
        cmd.Parameters.AddWithValue("type", meta.Type);
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileMeta>();
        return result.First();
    }
    
    /*public async Task<List<FileMeta>> GetAllReflectionAsync() // todo: may do later
    {
        var cmd = await CreateCommandAsync("SELECT * FROM file_meta");
        await using var reader = await cmd.ExecuteReaderAsync();

        var meta = new List<FileMeta>();

        var genericType = this.GetType().BaseType.GetGenericArguments()[0];
        
        while (await reader.ReadAsync())
        {
            FileMeta m = new FileMeta();
            foreach (var property in genericType.GetProperties())
            {
                int ordinal = property.GetCustomAttribute<OrdinalAttribute>()!.Ordinal;
                
                MethodInfo method = typeof(DbDataReader).GetMethod(nameof(DbDataReader.GetFieldValueAsync));
                MethodInfo generic = method.MakeGenericMethod(property.GetType());
                var dbValue = generic.Invoke(reader, new[] { ordinal as object });
            
                
                //var dbValue = reader.GetFieldValueAsync<>(ordinal);
            }
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
    }*/

    public async Task<List<DbFileMeta>> GetAllAsync()
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_meta");
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileMeta>();
        return result;
    }

    public async Task<List<DbFileMeta>> GetNextOffsetAsync(int offset, int next)
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_meta OFFSET @offset FETCH NEXT @next ROWS ONLY;");
        cmd.Parameters.AddWithValue("offset", offset);
        cmd.Parameters.AddWithValue("next", next);

        var result = await cmd.ExecuteAutoReaderAsync<DbFileMeta>();
        return result;
    }
    
    public async Task<DbFileNameDataTypeJoin> GetFileNameDataTypeJoin(Guid id)
    { 
        var cmd = _dataSource.CreateCommand("SELECT file_meta.name, file_data.data, file_meta.type FROM file_meta JOIN file_data ON file_meta.id = file_data.meta_id WHERE file_meta.id = @id;");
        cmd.Parameters.AddWithValue("id", id);
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileNameDataTypeJoin>();
        return result.First();
    }

    public async Task<DbFileDataMetaJoin> GetFileDataMetaJoin(Guid metaId)
    {
        var cmd = _dataSource.CreateCommand("SELECT file_meta.*, file_data.id data_id, file_data.data FROM file_meta JOIN file_data ON file_meta.id = file_data.meta_id WHERE file_meta.id = @id;");
        cmd.Parameters.AddWithValue("id", metaId);
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileDataMetaJoin>();
        return result.First();
    }
}