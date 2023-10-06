using System.Data;
using System.Reflection;
using FileHosting.DataAccess.Attributes;
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

    /*public async Task<List<T>> ExecuteReaderAsync<T>(string sql)
    {
        var cmd = new NpgsqlCommand(sql);
        await using var reader = await cmd.ExecuteReaderAsync();

        var type = typeof(T);
        var props = type.GetProperties();

        var tInstance = Activator.CreateInstance<T>();

        foreach (var prop in props)
        {
            var attribute = prop.GetCustomAttribute<FieldAttribute>();
            if (attribute == null) continue;

            string column = attribute.Title;
            object value = reader.GetFieldValueAsync<>()
        }
    }*/


}