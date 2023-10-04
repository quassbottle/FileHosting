using FileHosting.DataAccess.Repositories;
using FileHosting.Domain.Dto;
using FileHosting.Domain.Models;
using FileHosting.Domain.Services.Interfaces;
using FileHosting.Extensions;
using FileHosting.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileHosting.Controllers;

[ApiController]
[Route("aaa")]
public class WeatherForecastController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;
    
    public WeatherForecastController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }
    
    [HttpGet]
    public async Task Get()
    {
        await HttpContext.InitSseStreamAsync();
        int i = 5;
        while (i-- > 0)
        {
            await HttpContext.SendSseEventAsync(
                new SseEvent("test event", new { Penis = "penis", Balls = "balls" })
                {
                    Id = Guid.NewGuid().ToString(),
                    Retry = 10
                });
            await Task.Delay(500);
            i--;
        }
    }

}