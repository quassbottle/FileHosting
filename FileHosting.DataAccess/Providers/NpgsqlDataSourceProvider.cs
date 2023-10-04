using Npgsql;

namespace FileHosting.DataAccess.Providers;

public class NpgsqlDataSourceProvider
{
    private readonly string _connectionString;
    
    public NpgsqlDataSourceProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public NpgsqlDataSource GetDataSource()
    {
        return NpgsqlDataSource.Create(_connectionString);
    }
}