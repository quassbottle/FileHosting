using System.Data.Common;
using Npgsql;

namespace FileHosting.DataAccess.Repositories;

public abstract class NpgsqlRepository<T> : IDisposable, IAsyncDisposable
{
    private NpgsqlDataSource _dataSource;
    
    protected NpgsqlRepository(string connectionString)
    {
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    protected async Task ExecuteNonQueryAsync(string query)
    {
        /*await using (var cmd = _dataSource.CreateCommand("INSERT INTO data (some_field) VALUES ($1)"))
        {
            cmd.Parameters.AddWithValue("Hello world");
            await cmd.ExecuteNonQueryAsync();
        }*/
        await using var cmd = _dataSource.CreateCommand(query);
        await cmd.ExecuteNonQueryAsync();
    }
    
    protected async Task<NpgsqlCommand> CreateCommandAsync(string query)
    {
        return await Task.FromResult(_dataSource.CreateCommand(query));
    }
    
    
    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
}