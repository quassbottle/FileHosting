using System.Net;
using System.Text.Json;

namespace FileHosting.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;    
    
    public GlobalExceptionHandlerMiddleware(RequestDelegate next)    
    {    
        _next = next;    
    }    
    
    public async Task Invoke(HttpContext context)    
    {    
        try    
        {    
            await _next.Invoke(context);    
        }    
        catch (Exception ex)    
        {    
            await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);  
        }    
    }    
    
    private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)  
    {  
        context.Response.ContentType = "application/json";  
        int statusCode = (int)HttpStatusCode.InternalServerError;  
        var result = JsonSerializer.Serialize(new  
        {  
            status = statusCode,  
            message = exception.Message  
        });  
        context.Response.ContentType = "application/json";  
        context.Response.StatusCode = statusCode;  
        return context.Response.WriteAsync(result);  
    } 
}