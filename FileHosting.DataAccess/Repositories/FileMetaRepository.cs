using FileHosting.Domain.Entities;

namespace FileHosting.DataAccess.Repositories;

public class FileMetaRepository : NpgsqlRepository<DbFileMeta>
{
    public FileMetaRepository(string connectionString) : base(connectionString)
    {
    }

    public override async Task<DbFileMeta> UpdateAsync(DbFileMeta meta, Guid id)
    {
        var cmd = await CreateCommandAsync("UPDATE file_meta SET size = @size, name = @name WHERE id = @id RETURNING *;");

        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("size", meta.Size);
        cmd.Parameters.AddWithValue("name", meta.Name);   
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var size = reader.GetFieldValueAsync<long>(1);
        var name = reader.GetFieldValueAsync<string>(2);
        
        Task.WaitAll(guid, size, name);
        
        return new DbFileMeta
        {
            Guid = guid.Result,
            Name = name.Result,
            Size = size.Result
        };
    }

    public override async Task<int> DeleteByGuid(Guid id)
    {
        var cmd = await CreateCommandAsync("DELETE FROM file_meta WHERE id = $1;");
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public override async Task<DbFileMeta> FindByGuidAsync(Guid id)
    {
        var cmd = await this.CreateCommandAsync("SELECT * FROM file_meta WHERE id = $1");
        
        cmd.Parameters.AddWithValue(id);
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var size = reader.GetFieldValueAsync<long>(1);
        var name = reader.GetFieldValueAsync<string>(2);
        
        Task.WaitAll(guid, size, name);
        
        return new DbFileMeta
        {
            Guid = guid.Result,
            Name = name.Result,
            Size = size.Result
        };
    }

    public override async Task<DbFileMeta> CreateAsync(DbFileMeta meta)
    {
        var cmd = await CreateCommandAsync("INSERT INTO file_meta (size, name) VALUES (@size, @name) RETURNING *;");
        
        cmd.Parameters.AddWithValue("size", meta.Size);
        cmd.Parameters.AddWithValue("name", meta.Name);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        
        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>(0);
        var size = reader.GetFieldValueAsync<long>(1);
        var name = reader.GetFieldValueAsync<string>(2);
        
        Task.WaitAll(guid, size, name);
        
        return new DbFileMeta
        {
            Guid = guid.Result,
            Name = name.Result,
            Size = size.Result
        };
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

    public override async Task<List<DbFileMeta>> GetAllAsync()
    {
        var cmd = await CreateCommandAsync("SELECT * FROM file_meta");
        await using var reader = await cmd.ExecuteReaderAsync();

        var meta = new List<DbFileMeta>();

        while (await reader.ReadAsync())
        {
            var guid = reader.GetFieldValueAsync<Guid>(0);
            var size = reader.GetFieldValueAsync<long>(1);
            var name = reader.GetFieldValueAsync<string>(2);
            
            Task.WaitAll(guid, size, name);
            
            meta.Add(new DbFileMeta
            {
                Guid = guid.Result,
                Name = name.Result,
                Size = size.Result
            });
        }

        return meta;
    }
}