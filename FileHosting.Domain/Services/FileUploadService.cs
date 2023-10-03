using FileHosting.Domain.Dto;
using FileHosting.Domain.Models;
using FileHosting.Domain.Services.Interfaces;

namespace FileHosting.Domain.Services;

public class FileUploadService : IFileUploadService
{
    public Task<FileMeta> UploadFile(FullFileDto fileDto)
    {
        throw new NotImplementedException();
    }
}