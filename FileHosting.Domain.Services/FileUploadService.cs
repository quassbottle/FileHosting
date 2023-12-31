﻿using FileHosting.DataAccess.Entities;
using FileHosting.DataAccess.Repositories.Interfaces;
using FileHosting.Domain.Dto;
using FileHosting.Domain.Exceptions;
using FileHosting.Domain.Models;
using FileHosting.Domain.Services.Interfaces;

namespace FileHosting.Domain.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IFileMetaRepository _fileMetaRepository;
    private readonly IFileDataRepository _fileDataRepository;
    private readonly IFileUrlRepository _fileUrlRepository;

    public FileUploadService(IFileMetaRepository fileMetaRepository, 
        IFileDataRepository fileDataRepository,
        IFileUrlRepository fileUrlRepository)
    {
        _fileMetaRepository = fileMetaRepository;
        _fileDataRepository = fileDataRepository;
        _fileUrlRepository = fileUrlRepository;
    }

    public async Task<List<DbFileMeta>> GetUploadedFilesAsync()
    {
        return await _fileMetaRepository.GetAllAsync();
    }
    
    public async Task<DownloadFileDto> DownloadFileByIdAsync(Guid fileId)
    {
        var file = await _fileMetaRepository.GetFileNameDataTypeJoin(fileId);
        if (file is null) throw new FileNotFoundException($"File with UUID {fileId} has not been found");
        return new DownloadFileDto
        {
            Name = file.Name,
            Data = file.Data,
            Type = file.Type
        };
    }

    public async Task<DownloadFileDto> DownloadFileByUrlAsync(Guid urlId)
    {
        var dbUrl = await _fileUrlRepository.FindByGuidAsync(urlId);
        var dbFileNameDataType = await _fileUrlRepository.GetFileNameDataTypeJoin(dbUrl.Id);
        var dto = new DownloadFileDto
        {
            Data = dbFileNameDataType.Data,
            Name = dbFileNameDataType.Name,
            Type = dbFileNameDataType.Type
        };
        await _fileUrlRepository.DeleteByGuid(urlId);
        return dto;
    }

    public async Task<List<DbFileMeta>> GetNextOffsetAsync(FileMetaPageRequestDto request)
    {
        return await _fileMetaRepository.GetNextOffsetAsync(request.Offset, request.Next);
    }

    public async Task<FileUrlDto> GenerateUrlAsync(Guid fileId)
    {
        var dbUrl = await _fileUrlRepository.CreateAsync(new DbFileUrl
        {
            FileDataId = fileId
        });
        var dbFileNameDataType = await _fileUrlRepository.GetFileNameDataTypeJoin(dbUrl.Id);

        return new FileUrlDto
        {
            UrlId = dbUrl.Id,
            Name = dbFileNameDataType.Name
        };
    }
    
    public async Task<FileUploadedDto> UploadFileAsync(FileModel fileModel)
    {
        var dbFileMeta = await _fileMetaRepository.CreateAsync(new DbFileMeta
        {
            Name = fileModel.Name,
            Size = fileModel.SizeInBytes,
            Type = fileModel.Type
        });
        var dbFileData = await _fileDataRepository.CreateAsync(new DbFileData
        {
            FileMetaId = dbFileMeta.Id,
            Data = fileModel.Data
        });

        var result = await _fileMetaRepository.GetFileDataMetaJoin(dbFileMeta.Id);

        return new FileUploadedDto
        {
            Name = fileModel.Name,
            FileType = fileModel.Type,
            SizeInBytes = fileModel.SizeInBytes,
            IsUploaded = result != null,
            Id = result?.Id ?? Guid.Empty
        };
    }
}