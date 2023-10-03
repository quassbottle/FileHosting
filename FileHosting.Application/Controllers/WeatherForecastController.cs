using FileHosting.Extensions;
using FileHosting.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileHosting.Controllers;

[ApiController]
[Route("")]
public class WeatherForecastController : ControllerBase
{
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