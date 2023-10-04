using FileHosting.DataAccess.Repositories.Interfaces;
using FileHosting.Domain.Dto;
using FileHosting.Domain.Entities;
using FileHosting.Domain.Models;
using FileHosting.Domain.Services.Interfaces;

namespace FileHosting.Domain.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IFileMetaRepository _fileMetaRepository;
    private readonly IFileDataRepository _fileDataRepository;

    public FileUploadService(IFileMetaRepository fileMetaRepository, 
        IFileDataRepository fileDataRepository)
    {
        _fileMetaRepository = fileMetaRepository;
        _fileDataRepository = fileDataRepository;
    }
    
    public async Task<FileUploadedDto> UploadFile(FullFileDto fileDto)
    {
        var dbFileMeta = await _fileMetaRepository.CreateAsync(new DbFileMeta
        {
            Name = fileDto.Name,
            Size = fileDto.SizeInBytes,
            Type = fileDto.Type
        });
        var dbFileData = await _fileDataRepository.CreateAsync(new DbFileData
        {
            FileMetaId = dbFileMeta.Id,
            Data = fileDto.Content
        });

        var result = await _fileMetaRepository.GetFileDataAndMetaJoinById(dbFileData.Id);

        return new FileUploadedDto
        {
            Name = result.Name,
            FileType = result.Type,
            Id = result.Id,
            Data = result.Data,
            DataId = result.DataId,
            SizeInBytes = result.Size
        };
    }
}