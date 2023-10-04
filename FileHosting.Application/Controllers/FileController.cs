using FileHosting.DataAccess.Entities;
using FileHosting.Domain.Dto;
using FileHosting.Domain.Models;
using FileHosting.Domain.Services.Interfaces;
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
    public async Task<ActionResult<List<FileUploadedDto>>> LoadManyFiles(IFormFileCollection formFiles)
    {
        var uploadedFiles = new List<FileUploadedDto>();
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
            
            var fileUploaded = await _fileUploadService.UploadFile(file);

            uploadedFiles.Add(fileUploaded);
        }

        return Ok(uploadedFiles);
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

        return await _fileUploadService.UploadFile(fileDto);
    }

    [HttpGet("files")]
    public async Task<ActionResult<List<DbFileMeta>>> GetUploadedFiles()
    {
        return await _fileUploadService.GetUploadedFiles();
    }
    
    [HttpGet("files/{id}")]
    public async Task<ActionResult> DownloadFile([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) return BadRequest("Invalid file ID");

        var file = await _fileUploadService.DownloadFileById(guid);

        return File(file.Data, file.Type, file.Name);
    }

    [HttpGet("download/{id}")]
    public async Task<ActionResult<DownloadFileDto>> DownloadFileByUrl([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) return BadRequest("Invalid URL");
        
        var file = await _fileUploadService.DownloadFileByUrl(guid);
        if (file is null) return BadRequest("Invalid URL");
        
        return File(file.Data, file.Type, file.Name);
    }

    [HttpGet("getUrl/{id}")]
    public async Task<ActionResult<FileUrlDto>> CreateUrl([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) return BadRequest("Invalid file ID");
        
        var url = await _fileUploadService.GenerateUrl(guid);

        return url;
    }
}