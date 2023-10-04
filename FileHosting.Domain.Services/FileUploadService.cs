using FileHosting.DataAccess.Entities;
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

    public async Task<DownloadFileDto> DownloadFileById(Guid fileId)
    {
        var file = await _fileMetaRepository.GetFileNameDataJoinById(fileId);
        if (file is null) throw new FileNotFoundException($"File with UUID {fileId} has not been found");
        return new DownloadFileDto
        {
            Name = file.Name,
            Data = file.Data
        };
    }
    
    private async Task GenerateUrl(FileUploadedDto fileDto)
    {
        var url = await _fileUrlRepository.CreateAsync(new DbFileUrl
        {
            FileDataId = fileDto.DataId
        });
        
    }
    
    public async Task<FileUploadedDto> UploadFile(FileModel fileModel)
    {
        var t = await _fileUrlRepository.FindByGuidAsync(Guid.NewGuid());
        var dbFileMeta = await _fileMetaRepository.CreateAsync(new DbFileMeta
        {
            Name = fileModel.Name,
            Size = fileModel.SizeInBytes,
            Type = fileModel.Type
        });
        var dbFileData = await _fileDataRepository.CreateAsync(new DbFileData
        {
            FileMetaId = dbFileMeta.Id,
            Data = fileModel.Content
        });

        var result = await _fileMetaRepository.GetFileDataAndMetaJoinById(dbFileMeta.Id);

        return new FileUploadedDto
        {
            Name = result.Name,
            FileType = result.Type,
            Id = result.Id,
            DataId = result.DataId,
            SizeInBytes = result.Size
        };
    }
}