using FileHosting.DataAccess.Entities;
using FileHosting.Domain.Dto;
using FileHosting.Domain.Models;

namespace FileHosting.Domain.Services.Interfaces;

public interface IFileUploadService
{
    Task<FileUploadedDto> UploadFileAsync(FileModel fileModel);
    Task<DownloadFileDto> DownloadFileByIdAsync(Guid fileId);
    Task<List<DbFileMeta>> GetUploadedFilesAsync();
    Task<FileUrlDto> GenerateUrlAsync(Guid fileId);
    Task<DownloadFileDto> DownloadFileByUrlAsync(Guid urlId);
    Task<List<DbFileMeta>> GetNextOffsetAsync(FileMetaPageRequestDto request);
}