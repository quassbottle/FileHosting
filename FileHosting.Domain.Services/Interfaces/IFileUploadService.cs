using FileHosting.DataAccess.Entities;
using FileHosting.Domain.Dto;
using FileHosting.Domain.Models;

namespace FileHosting.Domain.Services.Interfaces;

public interface IFileUploadService
{
    Task<FileUploadedDto> UploadFile(FileModel fileModel);
    Task<DownloadFileDto> DownloadFileById(Guid fileId);
    Task<List<DbFileMeta>> GetUploadedFiles();
    Task<FileUrlDto> GenerateUrl(Guid fileId);
    Task<DownloadFileDto> DownloadFileByUrl(Guid urlId);
}