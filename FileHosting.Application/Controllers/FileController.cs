using System.Collections;
using FileHosting.DataAccess.Entities;
using FileHosting.Domain.Dto;
using FileHosting.Domain.Models;
using FileHosting.Domain.Services.Interfaces;
using FileHosting.Extensions;
using FileHosting.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileHosting.Controllers;

[ApiController]
[Route("")]
public class FileController : Controller
{
    private readonly IFileUploadService _fileUploadService;

    public FileController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }
    
    [HttpPost("uploadMany")]
    public async Task LoadManyFiles(IFormFileCollection formFiles)
    {
        //var uploadedFiles = new List<FileUploadedDto>();
        
        foreach (var formFile in formFiles)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            var file = new FileModel
            {
                Name = formFile.FileName,
                SizeInBytes = formFile.Length,
                Type = formFile.ContentType,
                Data = memoryStream.ToArray()
            };
            
            var fileUploaded = await _fileUploadService.UploadFileAsync(file);
            await HttpContext.SendSseEventAsync(new SseEvent("file_uploaded", fileUploaded));
        }
        //return Ok(uploadedFiles);
    }
    
    [HttpPost("upload")]
    public async Task<ActionResult<FileUploadedDto>> UploadFile(IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        
        var fileDto = new FileModel
        {
            Name = formFile.FileName,
            Type = formFile.ContentType,
            SizeInBytes = formFile.Length,
            Data = memoryStream.ToArray()
        };

        return Ok(await _fileUploadService.UploadFileAsync(fileDto));
    }

    [HttpGet("files")]
    public async Task<ActionResult<List<DbFileMeta>>> GetUploadedFiles()
    {
        return await _fileUploadService.GetUploadedFilesAsync();
    }
    
    [HttpPost("files/page")]
    public async Task<ActionResult<List<DbFileMeta>>> GetUploadedFiles(FileMetaPageRequestDto request)
    {
        return await _fileUploadService.GetNextOffsetAsync(request);
    }
    
    [HttpGet("files/{id}")]
    public async Task<ActionResult> DownloadFile([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) throw new ArgumentException("Invalid file ID");

        var file = await _fileUploadService.DownloadFileByIdAsync(guid);

        return File(file.Data, file.Type, file.Name);
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFileByUrl([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) throw new ArgumentException("Invalid file URL");
        
        var file = await _fileUploadService.DownloadFileByUrlAsync(guid);
        
        return File(file.Data, file.Type, file.Name);
    }

    [HttpGet("getUrl/{id}")]
    public async Task<ActionResult<FileUrlDto>> CreateUrl([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) throw new ArgumentException("Invalid file ID");
        
        var url = await _fileUploadService.GenerateUrlAsync(guid);

        return Ok(url);
    }
}