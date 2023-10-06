using System.Data;
using System.Data.SqlTypes;
using System.Reflection;
using System.Runtime.CompilerServices;
using FileHosting.DataAccess.Attributes;
using Npgsql;

namespace FileHosting.DataAccess.Extensions;

public static class NpgsqlReaderExtensions
{
    public static async Task<object> GetValueAsync(this NpgsqlDataReader reader, Type t, string name)
    {
        object value = await Task.Run(() => reader.GetValue(name));
        return value;
    }

    public static async Task<List<T>> ExecuteAutoReaderAsync<T>(this NpgsqlCommand cmd)
    {
        await using var reader = await cmd.ExecuteReaderAsync();

        if (!reader.HasRows) throw new SqlNullValueException($"Nothing found on request");

        var list = new List<T>();

        while (await reader.ReadAsync())
        {
            var instance = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                var attribute = prop.GetCustomAttribute<ColumnAttribute>();
                if (attribute == null) continue;
                string column = attribute.Title;
                object value = await reader.GetValueAsync(prop.GetType(), column);
                prop.SetValue(instance, value);
            }
            list.Add(instance);
        }

        return list;
        /*
        cmd.Parameters.AddWithValue(id);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!reader.HasRows) return null;

        await reader.ReadAsync();
        var guid = reader.GetFieldValueAsync<Guid>("id");
        var dataId = reader.GetFieldValueAsync<Guid>("meta_id");

        Task.WaitAll(guid, dataId);

        return new DbFileUrl()
        {
            Id = guid.Result,
            FileDataId = dataId.Result
        };

         */
    }
}