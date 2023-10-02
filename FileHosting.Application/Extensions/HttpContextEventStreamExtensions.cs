using System.Text.Json;
using FileHosting.Models;

namespace FileHosting.Extensions;

public static class HttpContextEventStreamExtensions
{
    public static async Task InitSseStreamAsync(this HttpContext ctx)
    {
        ctx.Response.Headers.Add("Cache-Control", "no-cache");
        ctx.Response.Headers.Add("Content-Type", "text/event-stream");
        await ctx.Response.Body.FlushAsync();
    }
    
    public static async Task SendSseEventAsync(this HttpContext ctx, SseEvent e)
    {
        if(String.IsNullOrWhiteSpace(e.Id) is false)
            await ctx.Response.WriteAsync("id: " + e.Id + "\n");

        if(e.Retry is not null)
            await ctx.Response.WriteAsync("retry: " + e.Retry + "\n");

        await ctx.Response.WriteAsync("event: " + e.Name + "\n");

        var lines = e.Data switch
        {
            null        => new [] { String.Empty },
            _           => new [] { JsonSerializer.Serialize(e.Data) }
        };

        foreach(var line in lines)
            await ctx.Response.WriteAsync("data: " + line + "\n");

        await ctx.Response.WriteAsync("\n");
        await ctx.Response.Body.FlushAsync();
    }
}