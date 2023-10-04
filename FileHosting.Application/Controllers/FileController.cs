using FileHosting.DataAccess.Entities;
using FileHosting.Domain.Exceptions;
using FileHosting.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpGet]
    public async Task<ActionResult<List<DbFileMeta>>> GetUploadedFiles()
    {
        return await _fileUploadService.GetUploadedFiles();
    }
    
    [HttpGet("d/{id}")]
    public async Task<IActionResult> DownloadFile([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var guid)) return BadRequest();

        var file = await _fileUploadService.DownloadFileById(guid);

        return File(file.Data, file.Type, file.Name);
    }
}