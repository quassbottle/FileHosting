﻿using System.Data;
using System.Data.Common;
using System.Reflection;
using FileHosting.DataAccess.Entities;
using FileHosting.DataAccess.Extensions;
using FileHosting.DataAccess.Providers;
using FileHosting.DataAccess.Repositories.Interfaces;
using Npgsql;

namespace FileHosting.DataAccess.Repositories;

public class FileUrlRepository : IFileUrlRepository
{
    private readonly NpgsqlDataSource _dataSource;
    
    public FileUrlRepository(NpgsqlDataSourceProvider provider)
    {
        _dataSource = provider.GetDataSource();
    }

    public async Task<DbFileUrl> UpdateAsync(DbFileUrl url, Guid id)
    {
        var cmd = _dataSource.CreateCommand("UPDATE file_url SET meta_id = @meta_id WHERE id = @id RETURNING *;");

        cmd.Parameters.AddWithValue("meta_id", url.FileDataId);
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileUrl>();
        return result.First();
    }

    public async Task<int> DeleteByGuid(Guid id)
    {
        var cmd = _dataSource.CreateCommand("DELETE FROM file_url WHERE id = $1;");
        cmd.Parameters.AddWithValue(id);
        return await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task<DbFileUrl> FindByGuidAsync(Guid id)
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_url WHERE id = $1");
        
        cmd.Parameters.AddWithValue(id);
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileUrl>();
        return result.First();
    }

    public async Task<DbFileUrl> CreateAsync(DbFileUrl url)
    {
        var cmd = _dataSource.CreateCommand(
            "INSERT INTO file_url (id, meta_id) VALUES (DEFAULT, @meta_id) RETURNING *;");

        cmd.Parameters.AddWithValue("meta_id", url.FileDataId);
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileUrl>();
        return result.First();
    }

    public async Task<List<DbFileUrl>> GetAllAsync()
    {
        var cmd = _dataSource.CreateCommand("SELECT * FROM file_url");
        
        var result = await cmd.ExecuteAutoReaderAsync<DbFileUrl>();
        return result;
    }

    public async Task<DbFileNameDataTypeJoin> GetFileNameDataTypeJoin(Guid id)
    { 
        var cmd = _dataSource.CreateCommand("SELECT fm.name, fd.data, fm.type FROM file_url fu JOIN file_meta fm ON fu.meta_id = fm.id JOIN file_data fd on fm.id = fd.meta_id WHERE fu.id = @id;");
        cmd.Parameters.AddWithValue("id", id);
        
        await using var reader = await cmd.ExecuteReaderAsync();

        await reader.ReadAsync();
        if (!reader.HasRows) return null;
        
        var name = reader.GetFieldValueAsync<string>("name");
        var data = reader.GetFieldValueAsync<byte[]>("data");
        var type = reader.GetFieldValueAsync<string>("type");
        
        Task.WaitAll(name, data, type);
        
        return new DbFileNameDataTypeJoin
        {
            Data = data.Result,
            Name = name.Result,
            Type = type.Result
        };
    }
}