using Npgsql;

namespace FileHosting.DataAccess.Repositories;

public abstract class NpgsqlRepository<T> : IDisposable, IAsyncDisposable
{
    private readonly NpgsqlDataSource _dataSource;
    
    protected NpgsqlRepository(string connectionString)
    {
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }
 
    protected async Task<NpgsqlCommand> CreateCommandAsync(string query)
    {
        return await Task.FromResult(_dataSource.CreateCommand(query));
    }

    public abstract Task<T> UpdateAsync(T t, Guid id);
    public abstract Task<int> DeleteByGuid(Guid id);
    public abstract Task<T> FindByGuidAsync(Guid id);
    public abstract Task<T> CreateAsync(T t);
    public abstract Task<List<T>> GetAllAsync();
    
    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
}